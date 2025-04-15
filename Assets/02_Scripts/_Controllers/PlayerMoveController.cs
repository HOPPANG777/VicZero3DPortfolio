using UnityEngine;
using System;


//==============================================================================
// PlayerMoveController
//==============================================================================
// �÷��̾��� �������� ����ϴ� ������Ʈ
// �÷��̾��� �̵� ����� CameraController�� ����.
//
//
//
//==============================================================================
public class PlayerMoveController : MonoBehaviour
{
    // �÷��̾� ������ٵ�
    [Header("Player")][SerializeField] private Rigidbody _playerRigidBody;
    public Rigidbody PlayerRigidBody { get { return _playerRigidBody; } }

    // �÷��̾� �ݶ��̴�
    [Header("Player")][SerializeField] private CapsuleCollider _playerCapsuleCollider;
    public CapsuleCollider PlayerCapsuleCollider { get { return _playerCapsuleCollider; } }
    
    // �÷��̾� �����̴� �ӵ�
    [Header("Player Movement")][SerializeField] private float _moveSpeed;
    public float PlayerMoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    // �÷��̾��� ������ ��
    [Header("Player Movement")][SerializeField] private float _jumpForce;
    public float PlayerJumpForce { get { return _jumpForce; } set { _jumpForce = value; } }

    // ���� �Է��� ���� �÷���
    [Header("Player Movement")][SerializeField] private bool _jumpRequested;
    public bool PlayerJumpRequested { get { return _jumpRequested; } set { _jumpRequested = value; } }

    void Start()
    {
        // �ʿ��� ������Ʈ�� �Ҵ���� �ʾҴٸ� GetComponent�� ���� ã���ϴ�.
        if (_playerRigidBody == null)
            _playerRigidBody = GetComponent<Rigidbody>();
        if (_playerCapsuleCollider == null)
            _playerCapsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // ���� �Է� (�����̽��ٰ� �⺻ ����)
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // ī�޶� ������ �̵� ó��
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // �Է� ���� ���� (���� ����)
        Vector3 inputVector = new Vector3(horizontal, 0f, vertical);

        if (inputVector.sqrMagnitude > 0.01f)
        {
            // ���� ī�޶� �������� �̵� ���� ��� (���� ������ ����)
            Transform camTransform = Camera.main.transform;
            Vector3 camForward = camTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = camTransform.right;
            camRight.y = 0f;
            camRight.Normalize();

            // �Է� ���͸� ī�޶� �������� ����
            Vector3 desiredMove = (camForward * inputVector.z + camRight * inputVector.x).normalized;
            Vector3 targetVelocity = desiredMove * _moveSpeed;

            // ���� y�ӵ�(�߷�, ���� ��)�� ������ ä xz �࿡�� ����
            Vector3 newVelocity = new Vector3(targetVelocity.x, _playerRigidBody.linearVelocity.y, targetVelocity.z);
            _playerRigidBody.linearVelocity = newVelocity;
        }
        else
        {
            // �Է��� ������ ���� �̵��� 0���� ����� ���ڸ� ���� (���� �ӵ��� ����)
            _playerRigidBody.linearVelocity = new Vector3(0f, _playerRigidBody.linearVelocity.y, 0f);
        }

        // ���� ��û�� �ִٸ�
        if (_jumpRequested)
        {
            // ���� ���� ���ʿ� ���޽� ���·� ���մϴ�.
            _playerRigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _jumpRequested = false;
        }
    }

    // ���� ������ ���� �Լ�
    // ĸ�� �ݶ��̴��� �߽ɿ��� �Ʒ� �������� ����ĳ��Ʈ�� ��� ���� �Ÿ� �� �浹�� ������ ���鿡 �ִٰ� �Ǵ��մϴ�.
    private bool IsGrounded()
    {
        // ĸ�� �ݶ��̴��� ��迡�� �Ʒ��� �ణ�� ����(extraHeight)�� ���� üũ�մϴ�.
        float extraHeight = 0.1f;
        Vector3 origin = _playerCapsuleCollider.bounds.center;
        float distance = _playerCapsuleCollider.bounds.extents.y + extraHeight;
        return Physics.Raycast(origin, Vector3.down, distance);
    }
}