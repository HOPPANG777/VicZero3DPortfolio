using UnityEngine;
using System;

//==============================================================================
// PlayerMoveController
//==============================================================================
// 플레이어의 움직임을 담당하는 컴포넌트
// 플레이어의 이동 방식은 CameraController를 참고.
//==============================================================================

public class PlayerMoveController : MonoBehaviour
{
    // 플레이어 리지드바디
    [Header("Player")]
    [SerializeField] private Rigidbody _playerRigidBody;
    public Rigidbody PlayerRigidBody { get { return _playerRigidBody; } }

    // 플레이어 콜라이더
    [Header("Player")]
    [SerializeField] private CapsuleCollider _playerCapsuleCollider;
    public CapsuleCollider PlayerCapsuleCollider { get { return _playerCapsuleCollider; } }

    // 플레이어가 이동하는 속도
    [Header("Player Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    public float PlayerMoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    // 플레이어의 점프시 힘
    [Header("Player Movement")]
    [SerializeField] private float _jumpForce = 5f;
    public float PlayerJumpForce { get { return _jumpForce; } set { _jumpForce = value; } }

    // 점프 입력을 위한 플래그
    private bool _jumpRequested = false;

    void Start()
    {
        // Start() 또는 Awake()에서 처리
        _playerRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // 필요한 컴포넌트가 할당되지 않았다면 GetComponent를 통해 찾습니다.
        if (_playerRigidBody == null)
            _playerRigidBody = GetComponent<Rigidbody>();
        if (_playerCapsuleCollider == null)
            _playerCapsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // 점프 입력 (스페이스바가 기본 설정)
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // 카메라 기준의 이동 처리
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 입력 벡터 생성 (수평만 고려)
        Vector3 inputVector = new Vector3(horizontal, 0f, vertical);

        if (inputVector.sqrMagnitude > 0.01f)
        {
            // 메인 카메라를 기준으로 이동 방향 계산 (수직 성분은 제거)
            Transform camTransform = Camera.main.transform;
            Vector3 camForward = camTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = camTransform.right;
            camRight.y = 0f;
            camRight.Normalize();

            // 입력 벡터를 카메라 방향으로 보정
            Vector3 desiredMove = (camForward * inputVector.z + camRight * inputVector.x).normalized;
            Vector3 targetVelocity = desiredMove * _moveSpeed;

            // 현재 y속도(중력, 점프 등)는 유지한 채 xz 축에만 적용
            Vector3 newVelocity = new Vector3(targetVelocity.x, _playerRigidBody.linearVelocity.y, targetVelocity.z);
            _playerRigidBody.linearVelocity = newVelocity;
        }
        else
        {
            // 입력이 없으면 수평 이동을 0으로 만들어 제자리 유지 (수직 속도는 유지)
            _playerRigidBody.linearVelocity = new Vector3(0f, _playerRigidBody.linearVelocity.y, 0f);
        }

        // 점프 요청이 있다면
        if (_jumpRequested)
        {
            // 점프 힘을 위쪽에 임펄스 형태로 가합니다.
            _playerRigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _jumpRequested = false;
        }
    }

    // 지면 판정을 위한 함수
    // 캡슐 콜라이더의 중심에서 아래 방향으로 레이캐스트를 쏘아 일정 거리 내 충돌이 있으면 지면에 있다고 판단합니다.
    private bool IsGrounded()
    {
        // 캡슐 콜라이더의 경계에서 아래로 약간의 여유(extraHeight)를 더해 체크합니다.
        float extraHeight = 0.1f;
        Vector3 origin = _playerCapsuleCollider.bounds.center;
        float distance = _playerCapsuleCollider.bounds.extents.y + extraHeight;
        return Physics.Raycast(origin, Vector3.down, distance);
    }
}
