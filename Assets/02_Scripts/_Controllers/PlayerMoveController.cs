using UnityEngine;
using System;
using UnityEngine.EventSystems;

//==============================================================================
// PlayerMoveController
//==============================================================================
// 플레이어의 움직임을 담당하는 컴포넌트
// 플레이어의 이동 방식은 CameraController를 참고.
//==============================================================================

public class PlayerMoveController : MonoBehaviour
{
    // 플레이어의 캐릭터 컨트롤러
    [Header("Player")]
    [SerializeField] private CharacterController _playerCharacterContoller;
    public CharacterController PlayerCharacterContoller { get { return _playerCharacterContoller; } }

    // 플레이어의 총
    [SerializeField] private GameObject _gunHolder;
    public GameObject GunHolder { get { return _gunHolder; } set { _gunHolder = value; } }

    // 플레이어가 이동하는 속도
    [Header("Player Movement")]
    [SerializeField] private float _moveSpeed; // 기본 5
    public float PlayerMoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    // 플레이어의 점프시 힘
    [SerializeField] private float _jumpForce; // 기본 5
    public float PlayerJumpForce { get { return _jumpForce; } set { _jumpForce = value; } }

    // 플레이어의 이동 방향
    [SerializeField] private Vector3 _moveDirection;
    public Vector3 PlayerMoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    // 중력
    [SerializeField] private float _gravity;
    public float PlayerGravity { get { return _gravity; } }

    // 점프 입력을 위한 플래그
    [SerializeField] private bool _jumpRequested; // 기본 false
    public bool PlayerJumpRequested { get { return _jumpRequested; } }

    void Start()
    {
        _playerCharacterContoller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (_playerCharacterContoller.isGrounded)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // 카메라 기준으로 방향 벡터 생성
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0.0f;
            forward.Normalize();

            Vector3 right = Camera.main.transform.right;
            right.y = 0.0f;
            right.Normalize();

            // 입력값을 카메라 방향으로 변환
            Vector3 desiredMove = (forward * vertical + right * horizontal).normalized;
            _moveDirection = desiredMove * _moveSpeed;
        }

        // 점프 입력
        if (Input.GetButtonDown("Jump"))
        {
            _moveDirection.y = _jumpForce;
        }

        {
            // 카메라의 회전을 GunHolder에 적용하여 총이 카메라와 함께 회전하도록 만듭니다.
            if (_gunHolder != null)
            {
                _gunHolder.transform.rotation = Camera.main.transform.rotation;
            }
        }

        // 중력 적용
        _moveDirection.y -= _gravity * Time.deltaTime;

        // 이동 실행
        _playerCharacterContoller.Move(_moveDirection * Time.deltaTime);
    }
}
