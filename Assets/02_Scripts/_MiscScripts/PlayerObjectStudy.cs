using UnityEngine;

public class PlayerObjectStudy : MonoBehaviour
{
    public float speed = 5f;
    public float speed_angular = 100f;

    //최초위치 : Vector3
    Vector3 pos_1st;
    //최초회전 : Quaternion
    Quaternion rot_1st;

    public bool isCanJump;
    //True : 점프 가능
    //false : 점프 불가능
    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pos_1st = transform.position;
        rot_1st = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    //>> 중력을 받는 오브젝트를 만들고, 해당 오브젝트가 필드에서 이동할 수 있도록 만들어보자!
    //키는 WASD -> 전진, 후진 , 좌회전 , 우회전 기능이 탑재되도록 제작!
    //키가 눌리고 있을 때 부드럽게 이동하도록 제작!
    //컴포넌트에서, 이동속도와 회전속도를 변경할 수 있도록 제작!
    public void Move()
    {
        //0. 플레이어가 이동하고 회전하는 이동속도, 회전속도를 외부에서 변경할 수 있도록 변수 선언!
        //1. WASD키가 눌리고 있음을 인식받을 수 있도록, if + Getkey구문 제작
        //2. 전진, 후진, 좌회전, 우회전에 맞게, transform 관련 함수들 구축!
        if (Input.GetKey(KeyCode.W))//전진
        {
            //+z
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))//후진
        {
            //-z
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))//좌회전
        {
            //-y
            transform.Rotate(0, -speed_angular * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))//우회전
        {
            //+y
            transform.Rotate(0, speed_angular * Time.deltaTime, 0);
        }

    }

    // >> AddForce를 통하여 점프 기능을 만들어보자!
    //점프키(Space)를 1회 누르면 1회 점프하는 기능!
    //점프? : 위쪽 방향으로 힘을 가하는 것!

    // -> 점프를 시작한 순간, 플레이어 오브젝트가 [땅에 닿기 전까지] 점프를 하지 않도록 막기!
    //1. 점프 가능 불가능 여부를 막아줄 수 있는 bool 변수 생성! 및 해당 변수를 통해 점프 기능 제한!
    //2. OnCollisionEnter 이벤트 함수를 통하여, 땅에/필드에/플레인에 닿았는지에 대한 여부를 체크!
    //   -> 만일 닿았다면 점프 가능 여부 초기화!
    public void Jump()
    {
        //1. 점프키가 한번 눌린것을 인식받을 수 있는 if + GetKeyDown구문 제작!
        //2. 힘을 가하는데 필요한 리지드바디를 가져옴!
        //3. 1에서 가져온 리지드바디를 통해 AddForce!
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //점프키가 눌렸다!
            if (isCanJump == true)
            {
                isCanJump = false;                
                rb.AddForce(500f * transform.up);
            }           
        }

    }





















    //-> 필드 아래 영역에 콜라이더를 생성 하여, 플레이어 오브젝트가 해당 영역을 인식(Trigger)하였다면 떨어졌다고 간주! 최초 위치/회전으로 초기화 하는 기능 생성
    //Trigger 이벤트 함수 활용! -> OnTriggerEnter!
    //최초 위치/회전은 어떻게? : 시작할때 위치/회전값을 저장!(Start!)
    void OnTriggerEnter(Collider other)
    {
        //0. 떨어짐 처리가 되었을 때, 초기화 시킬 최초 위치/회전값을 저장시킬 변수 선언 및 Start에서 저장
        //1. 만일 플레이어가 인식한 콜라이더가, 필드 하단부를 나타내는 콜라이더 인지에 대한 여부를 확인!(if!)
        //   -> 만일 플레이어가 인식한 콜라이더의 태그가, 필드 하단부를 나타내는 콜라이더의 태그(fall)인지 여부 확인!
        //1-1. 만일 1번이 참이라면, 플레이어 오브젝트의 위치와 회전을 초기화!

        if (other.tag == "fall")
        {
            //오브젝트가 필드 아래 있다! -> 떨어졌다!
            transform.position = pos_1st;
            transform.rotation = rot_1st;
        }
    }

    //2. OnCollisionEnter 이벤트 함수를 통하여, 땅에/필드에/플레인에 닿았는지에 대한 여부를 체크!
    //   -> 만일 땅에 닿았다면 점프 가능 여부 초기화!

    private void OnCollisionEnter(Collision collision)
    {
        //만일 땅에 닿았다면 -> 이 플레이어오브젝트가 닿은 오브젝트가 땅이라면
        //오브젝트가 땅인지는? 태그가 땅 태그!(ground)
        if(collision.collider.tag == "ground")
        {
            //땅하고 닿았다! 땅하고 충돌했다!
            isCanJump = true;
        }
    }



}
