using UnityEngine;
using UnityEngine.UI;

public class GameObjectStudy : MonoBehaviour
{
    public GameObject _gameObject;
    public Camera _camera;

    public InputField _input;
    public Text _text;

    public GameObject cube;

    public float velocity = 2f;
    public float velocity_ang = 50f;

    // Start is called before the first frame update
    void Start()
    {
        //GameObjectTest1();

        //GameObjectTest3();

        //GameObjectTest4();

        //PositionTest01();

        //PositionTest02();

        //  ScaleTest01();

        //ScaleTest02();

        //DirectionTest();
    }

    // Update is called once per frame
    void Update()
    {
        //MouseTest();

        //KeyBoardTest();

        //PositionTest03();

        //PositionTest04();

        //PositionTest05();

        //RotationTest01();

        //RotationTest02();

        //MoveTest01();

        MoveTest02();
    }

    //>> 게임오브젝트 및 컴포넌트를 변수화하여 해당 변수의 이름을 출력하는 함수 생성!
    //게임오브젝트 : 새로이 하나 생성해서 연결(cube!)
    //컴포넌트 : 카메라!
    public void GameObjectTest1()
    {
        //0. 게임오브젝트와 카메라 컴포넌트를, 외부에서 연결해줄 수 있도록 변수화!
        //1. 0에서 연결된 오브젝트와 컴포넌트의 이름을 출력!
        print(_gameObject);
        print(_camera);
    }

    //>>>> 입력창에 문구를 적고 버튼을 누르면 텍스트 컴포넌트에 입력된 문구가 나오는 구조 제작!!
    //스크립트에선, 버튼이 눌렸을 떄 실행시킬 함수를, 우리가 제작!
    //함수에서 갖춰야할 기능 : InputField 오브젝트에 입력된 문구를, Text 오브젝트에 출력!
    //-> 문구를 가져올 Inputfield 컴포넌트와, 문구를 출력할 Text 컴포넌트가 필요!
    public void GameObjectTest2()
    {
        //0. 문구를 가져올 Inputfield 컴포넌트와, 문구를 출력할 Text 컴포넌트를, 외부에서 연결할 수 있도록 변수 선언!
        //1. 0에서 선언된 InputField 컴포넌트에, 입력된 문자열을 받아옴!
        //2. 1에서 받아온 문자열을, 0에서 선언된 Text 컴포넌트에 출력!

        string msg;
        msg = _input.text;
        _text.text = msg;
    }

    // 컴포넌트를 통해 접근한 게임오브젝트와, 게임오브젝트를 통해 접근한 컴포넌트를 각각 출력해보자!
    public void GameObjectTest3()
    {
        //1. 게임 오브젝트를 통해, 해당 오브젝트 안에 들어있는 컴포넌트를 호출하여 해당 컴포넌트를 출력!
        //2. 컴포넌트(InputField)를 통해, 컴포넌트가 들어있는 오브젝트를 호출하여, 해당 오브젝트를 출력!

        var ren = _gameObject.GetComponent<MeshRenderer>();
        print(ren);
        GameObject obj = _input.gameObject;
        print(obj);
    }

    //>> 스크립트를 통해 게임오브젝트와 컴포넌트를 활성화/비활성화 시켜보자!
    //게임오브젝트는 활성화, 컴포넌트는 비활성화 테스트만 진행!
    //사용할 게임오브젝트는 _input , 사용할 컴포넌트는 _text
    public void GameObjectTest4()
    {
        //1. _input의 게임오브젝트를 SetActive 함수를 통해 활성화!
        //2. _text의 컴포넌트 비활성화
        _input.gameObject.SetActive(true);
        _text.enabled = false;

    }

    //>> 마우스 세 버튼이 눌렸는지 눌리고 있는지 떼어졌는지 여부를 체크할 수 있는 함수를 만들고 테스트 해보자!
    //키 입력 함수이기 때문에, if + update문과 결합!
    //각 버튼별로 3개의 상태가 있기 때문에, 총 9개의 if문 구조를 생성!
    public void MouseTest()
    {
        //왼쪽
        if (Input.GetMouseButtonDown(0))
        {
            print("마우스 왼쪽버튼 눌렸음!");
        }
        if (Input.GetMouseButton(0))
        {
            print("마우스 왼쪽버튼 눌리고 있음!");
        }
        if (Input.GetMouseButtonUp(0))
        {
            print("마우스 왼쪽버튼 떼어짐!");
        }
        //오른쪽
        if (Input.GetMouseButtonDown(1))
        {
            print("마우스 오른쪽버튼 눌렸음!");
        }
        if (Input.GetMouseButton(1))
        {
            print("마우스 오른쪽버튼 눌리고 있음!");
        }
        if (Input.GetMouseButtonUp(1))
        {
            print("마우스 오른쪽버튼 떼어짐!");
        }
        //휠
        if (Input.GetMouseButtonDown(2))
        {
            print("마우스 휠 버튼 눌렸음!");
        }
        if (Input.GetMouseButton(2))
        {
            print("마우스 휠 버튼 눌리고 있음!");
        }
        if (Input.GetMouseButtonUp(2))
        {
            print("마우스 휠 버튼 떼어짐!");
        }
    }

    //>>키보드 키 두개(스페이스키, 엔터키)가 눌렸는지 누르고 있는지 떼어졌는지 테스트 해보자!
    public void KeyBoardTest()
    {
        //스페이스
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("스페이스 키 눌림!");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            print("스페이스 키 눌리고 있음!");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            print("스페이스 키 떼어짐!");
        }
        //엔터
        if (Input.GetKeyDown(KeyCode.Return))
        {
            print("엔터키 눌림!");
        }
        if (Input.GetKey(KeyCode.Return))
        {
            print("엔터키 눌리고 있음!");
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            print("엔터키 떼어짐!");
        }
    }

    //>> 외부에서 받아온 오브젝트(cube)의 포지션을 출력!
    public void PositionTest01()
    {
        Vector3 pos;
        pos = cube.transform.position;
        print(pos);
    }

    //>> 외부에서 받아온 오브젝트의 위치 이동
    public void PositionTest02()
    {
        Vector3 pos1;
        pos1 = new Vector3(0, 0, 0);
        cube.transform.position = pos1;
    }

    //>> 방향키를 입력받아서, 외부에서 받아온 큐브 오브젝트의 위치 이동시켜보기!
    //방향키 입력 : if , update
    //방향키를 눌렀을 때, 상하좌우 1씩 이동하는 함수 제작
    public void PositionTest03()
    {
        //1. 상하좌우 방향키를 입력받을 수 있는, if + GetKeyDown구문 생성
        //2. 각 방향에 맞는 좌표 방향으로 1씩 위치 변경!
        if (Input.GetKeyDown(KeyCode.UpArrow))//위
        {
            //y+
            Vector3 pos = cube.transform.position;
            pos.y += 1;
            cube.transform.position = pos;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))//아래
        {
            //y-
            Vector3 pos = cube.transform.position;
            pos.y -= 1;
            cube.transform.position = pos;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))//좌
        {
            //x-
            Vector3 pos = cube.transform.position;
            pos.x -= 1;
            cube.transform.position = pos;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))//우
        {
            //x+
            Vector3 pos = cube.transform.position;
            pos.x += 1;
            cube.transform.position = pos;
        }
    }

    //>>> 진행했던 큐브 위치 이동 함수의 벡터값 변경을, 벡터간의 셈으로 변경하여 진행해보자!
    //함수 제작은 3번 함수의 복제 후, 안에 if문의 내용만 변경!
    public void PositionTest04()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))//위
        {
            //y+
            cube.transform.position += new Vector3(0, 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))//아래
        {
            //y-
            cube.transform.position -= new Vector3(0, 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))//좌
        {
            //x-
            cube.transform.position -= new Vector3(1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))//우
        {
            //x+
            cube.transform.position += new Vector3(1, 0, 0);
        }
    }

    //>> PositionTest03 함수를 Translate 함수를 통해 간소화 시켜보자!
    //PositionTest03함수를 복제하여, 05로 변경!
    //키 입력을 받는 if문 안의 내용만 변경!

    public void PositionTest05()
    {
        //1. 상하좌우 방향키를 입력받을 수 있는, if + GetKeyDown구문 생성
        //2. 각 방향에 맞는 좌표 방향으로 1씩 위치 변경!
        if (Input.GetKeyDown(KeyCode.UpArrow))//위
        {
            //y+
            cube.transform.Translate(0, 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))//아래
        {
            //y-
            cube.transform.Translate(0, -1, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))//좌
        {
            //x-
            cube.transform.Translate(-1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))//우
        {
            //x+
            cube.transform.Translate(1, 0, 0);
        }
    }

    //>>> 오일러 각을 통해서 큐브를 회전시켜보자!
    //방향키를 입력받았을 때 회전하는 코드 생성
    //좌방향키 : 좌회전(반시계)
    //우방향키 : 우회전(시계)
    public void RotationTest01()
    {
        //1. 방향키를 입력받을 수 있는 if+GetKeyDown구조 생성!
        //2. 각 방향키에 맞는 회전값 변경 실행!
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //z+ 15
            Vector3 eular = cube.transform.eulerAngles;
            eular.z += 15;
            cube.transform.eulerAngles = eular;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //z- 15
            cube.transform.eulerAngles -= new Vector3(0, 0, 15);
            //cube.transform.eulerAngles -= Vector3.forward * 15;
        }
    }

    //>> RotationTest01을 Rotate로 변경하여 함수를 만들어보자!
    //RotationTest01를 복제하여, 해당 키 입력 if문 구조를 제외한, if문 안의 내용들을 삭제
    public void RotationTest02()
    {
        //1. 방향키를 입력받을 수 있는 if+GetKeyDown구조 생성!
        //2. 각 방향키에 맞는 회전값 변경 실행!
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //z+ 15
            cube.transform.Rotate(0, 0, 15);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //z- 15
            cube.transform.Rotate(0, 0, -15);
        }
    }

    //>>> 큐브가 W/S키를 누르면 앞 뒤 이동, A/D키를 누르면 좌 우 회전을 하는 코드 생성
    //키 입력! update + if + getKeydown
    //이동은 Translate , 회전은 Rotate
    public void MoveTest01()
    {
        //1. W A S D 키를 입력받을수 있는 if + GetKeyDown 구조 생성
        //2. 각 키에 맞도록, 전진후진 , 좌회전 우회전 연결!
        //2-1. 전진후진 +y / -y
        //2-2. 좌우회전 +z / -z
        if (Input.GetKeyDown(KeyCode.W))//전진
        {
            //+y
            cube.transform.Translate(0, 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.S))//후진
        {
            //-y
            cube.transform.Translate(0, -1, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))//좌회전
        {
            //+z
            cube.transform.Rotate(0, 0, 15);
        }
        if (Input.GetKeyDown(KeyCode.D))//우회전
        {
            //-z
            cube.transform.Rotate(0, 0, -15);
        }
    }

    //>>> 큐브의 크기값을 불러와서 프린트
    public void ScaleTest01()
    {
        Vector3 scale = cube.transform.localScale;
        print(scale);
    }

    //>>> 큐브의 크기값을, 임의의 값으로 지정해보기!
    //임의의 크기값(3, 4, 5)
    public void ScaleTest02()
    {
        Vector3 scale = new Vector3(3, 4, 5);
        cube.transform.localScale = scale;
    }

    //>> 큐브의 6방향 방향벡터를 직접 출력해보고, 큐브를 임의로 회전시킨 후에 또 출력해보자!
    public void DirectionTest()
    {
        print(cube.transform.forward);  //전
        print(-cube.transform.forward); //후
        print(cube.transform.right);    //우
        print(-cube.transform.right);   //좌
        print(cube.transform.up);       //상
        print(-cube.transform.up);      //하
    }

    //>> Update와 Time.deltaTime을 통하여 오브젝트를 정해진 속도에 맞게 이동 및 회전시켜보자!
    //MoveTest01의 키 입력과 좌표 이동 및 회전 방향을 차용!
    //키 : W A S D
    //전진후진 +y / -y
    //좌우회전 +z / -z
    //정해진 속도과 각속도는, public으로 선언된 전역변수로 처리!
    public void MoveTest02()
    {
        //0. 속도와 각속도 값을 지니고 있는 변수를 외부에서 변경할 수 있도록 public 처리 하여 선언!
        //1. WASD키가 눌리고 있음을 입력받을 수 있는 if + GetKey구조 생성
        //2. 각 키의 입력에 맞는, 이동 및 회전 진행!(속도 * Time.deltaTime)

        if (Input.GetKey(KeyCode.W))//전진
        {
            //y+
            cube.transform.Translate(0, velocity * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))//후진
        {
            //y-
            cube.transform.Translate(0, -velocity * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.A))//좌회전
        {
            //z+
            cube.transform.Rotate(0, 0, velocity_ang * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))//우회전
        {
            //z-
            cube.transform.Rotate(0, 0, -velocity_ang * Time.deltaTime);
        }
    }
}
