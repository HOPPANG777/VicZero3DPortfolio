using UnityEngine;
using System;
using UnityEditor.Build;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
//using static Unity.Cinemachine.AxisState;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    // 카메라가 따라갈 대상 (플레이어 등)
    [Header("Target Object")][SerializeField] GameObject _targetObject;
    public GameObject TargetObject { get { return _targetObject; } set { _targetObject = value; } }

    // 타겟으로부터 3인칭 시점일 때의 오프셋 (예: (0, 2, -5))
    [Header("Third Person Offset")][SerializeField] Vector3 _thirdPersonOffset;
    public Vector3 ThirdPersonOffset { get { return _thirdPersonOffset; } set { _thirdPersonOffset = value; } }

    // 1인칭 시점(플레이어의 가슴 위치) 기준 오프셋
    [Header("First Person Offset (Chest)")][SerializeField] Vector3 _firstPersonOffset;
    public Vector3 FirstPersonOffset { get { return _firstPersonOffset; } set { _firstPersonOffset = value; } }

    // 3인칭과 1인칭 사이의 보간 계수
    // 1.0f : 완전 3인칭, 0.0f : 완전 1인칭
    [Range(0f, 1f)][Header("Camera Lerp")][SerializeField] float _currentLerp;
    public float CurrentLerp { get { return _currentLerp; } set { _currentLerp = value; } }

    // 마우스 이동에 따른 보간 값 변화 민감도
    [Header("Camera Lerp")][SerializeField] float _lerpSensitivity;
    public float LerpSensitivity { get { return _lerpSensitivity; } set { _lerpSensitivity = value; } }

    // 3인칭 모드에서 카메라가 플레이어를 내려다보도록 하는 추가 피치 (단위: 도)
    [Header("Additional Third Person Pitch (Downward Angle)")][SerializeField] float _additionalThirdPersonPitch;
    public float AdditionalThirdPersonPitch { get { return _additionalThirdPersonPitch; } set { _additionalThirdPersonPitch = value; } }

    // 1인칭 모드에서의 마우스 자유회전 감도
    [Header("First Person Look Sensitivity")][SerializeField] float _firstPersonLookSensitivityX;
    public float FirstPersonLookSensitivityX { get { return _firstPersonLookSensitivityX; } set { _firstPersonLookSensitivityX = value; } }

    [Header("First Person Look Sensitivity")][SerializeField] float _firstPersonLookSensitivityY;
    public float FirstPersonLookSensitivityY { get { return _firstPersonLookSensitivityY; } set { _firstPersonLookSensitivityY = value; } }

    [Header("Collision")][SerializeField] float _collisionOffset; // 카메라와 장애물 사이 최소 거리
    public float CollisionOffset { get { return _collisionOffset; } }

    [Header("Collision")][SerializeField] int _originalCullingMask; // 카메라와 장애물 사이 최소 거리
    public int OriginalCullingMask { get { return _originalCullingMask; } }

    [Header("Camera")][SerializeField] Camera _cam; // 카메라와 장애물 사이 최소 거리
    public Camera Cam { get { return _cam; } }

    // 내부적으로 1인칭 모드에서의 yaw, pitch 값을 저장
    private float _firstPersonYaw;
    private float _firstPersonPitch;

    void Start()
    {
        _cam = GetComponent<Camera>();
        _originalCullingMask = _cam.cullingMask;
        CameraInit();
    }

    void LateUpdate()
    {
        // 마우스 입력
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // [1인칭 자유회전 업데이트]
        CameraFreeRotate(mouseX, mouseY);

        // [전환 입력 처리]
        CameraInput(mouseY);

        // [카메라 위치 계산]
        // 1인칭 위치: 타겟 위치 + 1인칭 오프셋 (가슴 위치)
        Vector3 firstPersonPos = _targetObject.transform.position + _firstPersonOffset;
        // 3인칭 위치: 타겟 위치 + 타겟 로컬 회전을 적용한 3인칭 오프셋
        Vector3 thirdPersonPos = _targetObject.transform.position + _targetObject.transform.rotation * _thirdPersonOffset;
        // Lerp를 통해 최종 카메라 위치 계산
        Vector3 desiredPosition = Vector3.Lerp(firstPersonPos, thirdPersonPos, _currentLerp);

        // [충돌 처리]
        CameraOnCollider(desiredPosition, firstPersonPos);

        // [카메라 회전 처리]
        CameraRotate(firstPersonPos);

        // [카메라 컬링마스크]
        UpdateCullingMask();
    }

    void CameraInit()
    {
        if (_targetObject == null) return;

        // 초기 1인칭 회전 각도는 타겟의 회전에서 가져옵니다.
        Vector3 angles = _targetObject.transform.rotation.eulerAngles;
        _firstPersonYaw = angles.y;
        _firstPersonPitch = angles.x;
    }

    void CameraFreeRotate(float mouseX, float mouseY)
    {
        // _currentLerp가 0에 가까우면(1인칭 모드)만 자유 회전 값을 업데이트
        if (_currentLerp > 0.01f) return;

        _firstPersonYaw += mouseX * _firstPersonLookSensitivityX;
        _firstPersonPitch = Mathf.Clamp(_firstPersonPitch - mouseY * _firstPersonLookSensitivityY, -89f, 89f);
    }

    void CameraInput(float mouseY)
    {
        // V키와 좌클릭이 눌리지 않았다면 바로 반환
        if (!(Input.GetKey(KeyCode.V) && Input.GetMouseButton(0))) return;

        // 마우스 Y가 음수(뒤로 당기면) _currentLerp 증가: 3인칭 전환
        _currentLerp = Mathf.Clamp01(_currentLerp - mouseY * _lerpSensitivity);
    }

    void CameraOnCollider(Vector3 desiredPosition, Vector3 firstPersonPos)
    {
        Vector3 offsetDir = desiredPosition - firstPersonPos;
        float offsetDist = offsetDir.magnitude;

        // 카메라가 이동할 거리가 극히 작으면 바로 원하는 위치로 설정
        if (offsetDist <= 0.001f)
        {
            this.transform.position = desiredPosition;
            return;
        }

        offsetDir /= offsetDist; // 정규화

        // 첫번째 지점부터 원하는 위치까지 레이캐스트 후 장애물이 있으면 조정
        if (Physics.Raycast(firstPersonPos, offsetDir, out RaycastHit hit, offsetDist) && hit.collider.gameObject != _targetObject)
        {
            float adjustedDist = Mathf.Max(hit.distance - _collisionOffset, 0f);
            desiredPosition = firstPersonPos + offsetDir * adjustedDist;
        }

        this.transform.position = desiredPosition;
    }

    void CameraRotate(Vector3 firstPersonPos)
    {
        // 3인칭 모드
        if (_currentLerp > 0.01f)
        {
            Vector3 lookDir = firstPersonPos - transform.position;
            if (lookDir.sqrMagnitude <= 0.0001f) return;

            Quaternion baseRot = Quaternion.LookRotation(lookDir, Vector3.up);
            this.transform.rotation = baseRot * Quaternion.Euler(-_additionalThirdPersonPitch, 0f, 0f); return;
        }

        // 1인칭 모드: 자유 회전 값 적용
        this.transform.rotation = Quaternion.Euler(_firstPersonPitch, _firstPersonYaw, 0f);
    }

    void UpdateCullingMask()
    {
        //int playerLayer = LayerMask.NameToLayer("Player"); // 플레이어가 할당된 레이어 이름
        if (_currentLerp < 0.01f) // 1인칭 모드
        {
            // 원래 마스크에서 플레이어 레이어 제거
            //_cam.cullingMask = _originalCullingMask & ~(1 << playerLayer);
        }
        else // 3인칭 모드
        {
            //_cam.cullingMask = _originalCullingMask;
        }
    }

}

