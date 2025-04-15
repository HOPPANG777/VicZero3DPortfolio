using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStudy : MonoBehaviour
{
    public int i = 12312312;
    public float f = 3.14f;
    public string s = "안녕하세요!";

    public Vector3 vector;

    public int age;

    public string foodName;

    public enum Week
    {
        Mon,
        Tue,
        Wed,
        Thu,
        Fri,
        Sat,
        Sun
    }
    public Week week;

    private int[] int_array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    float[] float_array1 = new float[10];
    public float[] float_array2; //크기가 0

    public string[] string_array;

    public List<string> string_list = new List<string>();

    public List<int> int_list = new List<int>();

    public Dictionary<int, string> int_name_Dictionary = new Dictionary<int, string>();

    public Dictionary<string, int> studentGradeDictionary = new Dictionary<string, int>();

    public float p;



    // Start is called before the first frame update
    void Start()
    {
        //print("Hello Worlds!");
        //print(i);
        //print(f);
        //print(s);
        /*
        int ii;
        ii = IntF();
        print(ii);
        */

        //PrintDouble("왜 두번 쳐지지?");


        /*
        int r;
        r = PlusInt(111 , 333);
        print(r);
        */

        //print(vector.x);
        /*
        print(5 / 3);
        print(5.0 / 3.0);
        print(5 % 3);
        print(5.0 % 3.0);
        */
        //print(5 == 5);   //true
        //print(!(5 > 3)); //false
        //print(!true);    //false

        //Age();

        //Age2();

        //SwitchTest();

        //print(week);

        //EnumSwitchTest();


        //print((int)week);

        //ForTest();

        //ForTest2();

        //ForTest3();

        //WhileTest1();

        //WhileTest2();

        //DoWhileTest1();

        //DoWhileTest2();

        //BreakTest1();

        //WhileTrueTest();

        //ContinueTest1();

        //ContinueTest2();

        //ArrayTest1();

        //ArrayTest2();

        //ArrayTest3();

        //ForeachTest();

        //ListTest1();

        //ListTest2();

        //DictionaryTest01();

        //DictionaryTest2();

        //지역변수 , 전역변수
        float p = 12.456f;
        print(p);
        print(this.p);
    }

    // Update is called once per frame
    void Update()
    {
        //print(s);
    }

    /*
    public int IntF()
    {
        Debug.Log("IntF 함수 실행!");
        return i;
        Debug.Log("IntF 함수 리턴 실행!");
    } 
    */



    //인자로 받아온 문자열을 두번 출력해주는 함수!
    public void PrintDouble(string msg)
    {
        Debug.Log(msg);
        Debug.Log(msg);
    }

    //두개의 인자를 받아오는 함수 제작!
    //두개의 정수를 받아와서, 그 두 정수를 더해서 리턴해주는 함수
    public int PlusInt(int a, int b)
    {
        int result; //더한 값을 저장해줄 공간
        result = a + b;
        return result;
    }


    // if - else if - else 구문을 사용하여, 나이 변수를 통해 나이대를 로그로 콘솔에 출력하는 함수를 만들어보자!
    //해당 함수는 실행시 최초 1회만 실행! >> Start!
    //나이를 비교할 나이 변수는 public을 통해 인스펙터에서 받아옴!
    //출력할 나이대는, 10대 미만, 10대, 20대 , 30대 , 40대 이상 <<
    public void Age()
    {
        //0. 나이를 비교할 나이 변수를 선언!
        //1. 0에서 선언된 정수형 나이 변수를 통해 값을 비교하여, 나이대를 분류!
        //1-1. 10대 미만
        //1-2. 10대
        //1-3. 20대
        //1-4. 30대
        //1-5. 40대 이상
        //2. 각 조건에 맞는 나이에 따라, 문구 출력!
        //디버그 : 나이가 음수일 수 있나?
        //>> 나이가 음수일 때 조건을 추가!

        if (age < 0) //나이가 음수 : 나이가 음수일수 없음! 오류!
        {
            Debug.Log("나이값은 음수가 될 수 없습니다!");
        }
        else if (age < 10) //1-1. 10대 미만
        {
            //나이가 10보다 작을때
            Debug.Log("나이가 10대 미만입니다!");
        }
        else if (age < 20)  //1-2. 10대
        {
            //나이가 10 이상이고 20 미만일 때
            Debug.Log("나이가 10대 입니다!");
        }
        else if (age < 30)       //1-3. 20대
        {
            //나이가 20 이상이고 30 미만일 때
            Debug.Log("나이가 20대 입니다!");
        }
        else if (age < 40)       //1-4. 30대
        {
            //나이가 30 이상이고 40 미만일 때
            Debug.Log("나이가 30대 입니다!");
        }
        else//1-5. 40대 이상
        {
            //나이가 40보다 같거나 크다!
            Debug.Log("나이가 40대 이상입니다!");
        }

    }

    // 조건 연산자 + if문을 통하여, 나이대를 구분하는 함수를 바꿔보자!
    // 해당 함수는 실행시 최초 1회만 실행! >> Start!
    // 나이를 비교할 나이 변수는 public을 통해 인스펙터에서 받아옴!
    // 출력할 나이대는, 10대 미만, 10대, 20대 , 30대 , 40대 이상 <<

    public void Age2()
    {
        //1. age변수를 통하여 if문을 구축
        //1-1. 0보다 작을때 << 0보다 작은 값은 나이 X
        //1-2. 10대 미만 : 0보다 크거나 같고 10보다 작을 때
        //1-3. 10대 : 10보다 크거나 같고 20보다 작을 때
        //1-4. 20대 : 20보다 크거나 같고 30보다 작을 때
        //1-5. 30대 : 30보다 크거나 같고 40보다 작을 때
        //1-6. 40대 이상 : 40보다 크거나 같을 때
        //2. 각 IF문의 조건에 맞게, 문구를 출력!
        if (age < 0) //1-1. 0보다 작을때 << 0보다 작은 값은 나이 X
        {
            //나이 X
            Debug.Log("나이값은 음수가 될 수 없습니다!");
        }
        if (age >= 0 && age < 10)//1-2. 10대 미만 : 0보다 크거나 같다 그리고 10보다 작을 때
        {
            //10대 미만
            Debug.Log("나이가 10대 미만입니다!");
        }
        if (age >= 10 && age < 20)//1-3. 10대 : 10보다 크거나 같다 그리고 20보다 작을 때
        {
            // 10대
            Debug.Log("나이가 10대 입니다!");
        }
        if (age >= 20 && age < 30)//1-4. 20대 : 20보다 크거나 같고 30보다 작을 때
        {
            // 20대
            Debug.Log("나이가 20대 입니다!");
        }
        if (age >= 30 && age < 40)//1-5. 30대 : 30보다 크거나 같고 40보다 작을 때
        {
            // 30대
            Debug.Log("나이가 30대 입니다!");
        }
        if (age >= 40)//1-6. 40대 이상 : 40보다 크거나 같을 때
        {
            // 40대 이상 
            Debug.Log("나이가 40대 이상입니다!");
        }
    }

    // 문자열형 변수에 따라 다른 문구를 출력해주는 Switch문을 작성해보자!
    // 문자열형 변수는 외부에서 받아올 수 있도록 public으로 선언
    // 최초 1회만 실행시키기 위해서 해당 함수는 Start에다 연결
    public void SwitchTest()
    {
        Debug.Log("점심 뭐먹을까?");
        //0. 외부에서 문자열을 받아올 수 있는 문자열형 변수를 public 선언!
        //1. 0에서 선언된 문자열에 따라 Switch문 제작!
        //2. 비교된 문구에 따라 다른 문구 출력!
        switch (foodName)
        {
            case "짜장면":
                {
                    Debug.Log("느끼해서 별로...");
                    break;
                }
            case "파스타":
                {
                    Debug.Log("어제 먹었는데?");
                    break;
                }
            case "국밥":
                {
                    Debug.Log("너무 더부룩해.");
                    break;
                }
            case "돈까스":
                {
                    Debug.Log("그저께 먹었는데?");
                    break;
                }
            default:
                {
                    Debug.Log("점심먹기 싫어?");
                    break;
                }
        }
    }

    // enum형과 switch문을 결합해서 사용해보자!
    // 미리 만들었던, 요일을 나타내는 Enum형을 통하여 Switch문과 결합해 요일에 맞는 문구를 출력!
    //Start에 연결!
    public void EnumSwitchTest()
    {
        switch (week)
        {
            case Week.Mon:
                {
                    Debug.Log("월요일이네... 출근하기 싫다...");
                    break;
                }
            case Week.Tue:
                {
                    Debug.Log("화요일이네... 술먹고 싶다....");
                    break;
                }
            case Week.Wed:
                {
                    Debug.Log("수요일이네... 시간빠르네...");
                    break;
                }
            case Week.Thu:
                {
                    Debug.Log("뭐야. 언제 목요일 되었지??");
                    break;
                }
            case Week.Fri:
                {
                    Debug.Log("와! 퇴근하면!");
                    break;
                }
            case Week.Sat:
                {
                    Debug.Log("Zzz");
                    break;
                }
            case Week.Sun:
                {
                    Debug.Log("내일 출근이네....");
                    break;
                }
        }

    }


    //-> for문을 통해 특정 문구를 5번 출력하는 함수 생성
    //Start!

    public void ForTest()
    {
        for (int i = 1; i <= 5; i++)
        {
            Debug.Log("For문 테스트 중입니다!");
        }
    }

    // 1부터 100까지의 수를 출력하는 함수!
    public void ForTest2()
    {
        for (int i = 1; i <= 100; i++)
        {
            Debug.Log(i);
        }
    }

    //2) 1부터 100까지의 홀수를 출력하는 함수!
    //홀수 : 1 3 5 7 9 ...... 97 99
    //시작 : 1 , 수와 수 차이 : 2
    public void ForTest3()
    {
        for (int i = 1; i <= 100; i += 2)
        {
            Debug.Log(i);
        }
    }

    // 1부터 100까지의 수를 출력하는 함수!
    public void WhileTest1()
    {
        //1. 출력할 수를 담고있는 변수 선언!
        //2. While문 조건 비교
        //3. While문 내용 작성
        int num;
        num = 1;
        while (num <= 100) //100이라는 수를 출력했을 때까지 반복! 
        {
            //100일땐 참 101일땐 거짓
            Debug.Log(num);
            num++;
        }

    }

    //2) 1부터 100까지의 홀수를 출력하는 함수!
    public void WhileTest2()
    {
        //1. 출력할 수를 담고있는 변수 선언!
        //2. While문 내용 작성
        //-> 출력 후 다음 수 출력을 위한 숫자 증가!
        //-> 다음 수를 위해 증가할 숫자? 2
        //3. While문 조건 비교
        //1 3 5 ... 97 99

        int num2 = 1;
        while (num2 <= 100)
        {
            Debug.Log(num2);
            num2 += 2;
        }
    }

    //while문 1번 예제를 do-while문으로 바꿔서 작성해보자
    //1부터 100까지의 수를 출력하는 함수!
    public void DoWhileTest1()
    {
        //1. 출력할 수를 담고있는 변수 생성
        //2. dowhile문 작성
        //2-1. 실행할 내용 작성
        //2-2. 비교할 조건식 설정
        int num3 = 1;
        do
        {
            Debug.Log(num3);
            num3++;
        } while (num3 <= 100);
        //1~100까지의 수가 전부 만족하는 조건식

    }

    //2) 정말 조건식이 거짓이어도 한번은 실행이 될까?
    public void DoWhileTest2()
    {
        do
        {
            Debug.Log("do 실행되는가?");
        } while (1 == 2);

        //while (1 == 2)
        //{
        //    Debug.Log("while 실행되는가");
        //}
    }

    // 1부터 100까지의 수를 출력하는 반복문에서, 출력하는 수가 50일 때(if) break를 호출한다면?
    public void BreakTest1()
    {
        //1부터 100까지의 수를 출력하는 반복문
        for (int i1 = 1; i1 <= 100; i1++)
        {
            Debug.Log(i1);
            if (i1 == 50)
            {
                break;
            }

        }
    }

    //While(true)문과 Break문을 사용하여, 1부터 100까지의 수를 출력!
    public void WhileTrueTest()
    {
        //1. While(True)문 생성
        //2. 내용 부분 생성
        //3. 조건부분(내가 원하는 조건이 되었을때 Break를 통해 반복문을 종료!)을 제작

        int num_while = 1;

        while (true)
        {
            Debug.Log(num_while);
            num_while++;
            //조건부분
            //100까지 출력! -> 이후 반복문 종료!
            //num_while이 100일때까진 ok! / 101이 되는 순간 반복문 탈출!
            if (num_while > 100)
            {
                break;
            }
        }
    }

    //1. 진짜로 스킵하는가?
    public void ContinueTest1()
    {
        //1. for문을 통해 특정 4가지 문구를 5번 출력해주는 반복문 생성!
        //2. 두번째 내용과 세번째 내용 사이에 continue문 삽입
        for (int i_1 = 1; i_1 <= 5; i_1++)
        {
            Debug.Log("한번 출력!");
            Debug.Log("두번 출력!");
            continue;
            //Debug.Log("세번 출력!");
            //Debug.Log("네번 출력!");
        }
    }

    //2. continue문을 통해서, 1에서 100까지의 수 중에 홀수만 출력하는 반복문 생성
    public void ContinueTest2()
    {
        //1. 1부터 100까지의 수를 출력해주는 for형태 반복문 생성
        //2. 1에서의 반복문에서, 출력할 수가 홀수가 아니라면(짝수라면)  continue를 실행하는 조건식을 생성!

        for (int i = 1; i <= 100; i++)
        {
            //i가 짝수인지를 비교해서, 짝수라면 컨티뉴!
            if (i % 2 == 0)//짝수!
            {
                continue;
            }
            Debug.Log(i);
        }
    }

    //미리 만들어둔 배열의 값에 접근하여 안의 값을 출력해보자!

    public void ArrayTest1()
    {
        Debug.Log(int_array[0]); //index가 0 : 첫번째 요소에 접근!
        Debug.Log(int_array[1]);
        Debug.Log(int_array[2]);
        Debug.Log(int_array[3]);
        Debug.Log(int_array[4]);
        Debug.Log(int_array[5]);
        Debug.Log(int_array[6]);
        Debug.Log(int_array[7]);
        Debug.Log(int_array[8]);
        Debug.Log(int_array[9]);
        //print(int_array[10]);//Error!
    }

    //미리 만들어둔 배열의 값에 접근하여 안의 값을 출력해보자! + for를 통해서
    public void ArrayTest2()
    {
        //1. for반복문 생성
        //1-1. for 반복문 초기식 생성
        //1-2. for 반복문 내용 생성
        //1-3. for 반복문 반복식 생성
        //1-4. for 반복문 조건식 생성
        //-> i가 0~9일땐 참 , 10일땐 거짓
        //for (int i = 0; i <= 9; i++)
        //{
        //    print(int_array[i]);
        //}
        //만약, int_array의 요소의 갯수가 달라진다면?
        //위의 반복식은 요소의 갯수가 달라진다면 오류가 난다!

        for (int i = 0; i < int_array.Length; i++)
        {
            Debug.Log(int_array[i]);
        }

    }

    //초기화 한 배열과, 초기화 하지 않은 배열을 각각 사용해보자!
    //실수형 배열을 두개를 준비! 
    //1. 배열 선언 후 new 생성자를 통해 10의 크기로 초기화!
    //2. 초기화 X
    //두개의 배열에 첫번째 요소에 접근도 해보고, 값도 직접 넣어보자!
    public void ArrayTest3()
    {
        //0. 두개의 실수형 배열을 선언!
        //0-1. 첫번째 배열은 크기를 10으로 초기화!
        //0-2. 두번째 배열은 초기화 X
        //1. 첫번째 배열의 첫번째 요소에 값을 넣어보고, 해당 첫번째 요소를 출력!
        //2. 두번째 배열의 첫번째 요소에 값을 넣어보고, 해당 첫번째 요소를 출력!
        float_array1[0] = 101f;
        Debug.Log(float_array1[0]);

        float_array2[0] = 202f;
        Debug.Log(float_array2[0]);

    }

    // foreach를 통해서 외부에서 접근할 수 있는 문자열형 배열 안의 요소들을 출력해보자!
    // 배열은 문자열형 배열을 선언 하며, 외부에서 접근할 수 있도록 public으로 선언
    public void ForeachTest()
    {
        //0. 외부에서 접근할 수 있는 문자열형 배열 선언
        //1. 0의 배열을 통해 foreach문 구성
        //2. foreach문을 통해 배열안의 요소를 출력
        foreach (string s in string_array)
        {
            Debug.Log(s);
        }
    }

    //1) 리스트의 요소 추가 기능을 통해 원하는 요소들을 추가하고, 리스트 내부 요소들을 for문을 통해 콘솔창에 출력해보자!
    //리스트는 문자열형 리스트를 사용!
    public void ListTest1()
    {
        //0. 문자열형 리스트를 선언
        //1. 해당 리스트에 요소들을 추가(5개)
        //2. for문을 통해서 요소들에 접근하여 해당 요소들을 출력!
        string_list.Add("One");
        string_list.Add("Two");
        string_list.Add("Three");
        string_list.Add("Four");
        string_list.Add("Five");

        ///
        string_list.Remove("Three");
        string_list.Add("Three");

        //
        string_list.Clear();

        for (int index = 0; index < string_list.Count; index++)
        {

            Debug.Log(string_list[index]);
        }

    }

    //4) 새로이 만든 리스트를, public 기능을 통하여 리스트 내부 요소를 편집해보자
    //4-1) 4의 리스트의 내부의 요소들을 foreach문을 통하여 출력해보자!
    public void ListTest2()
    {
        //0. 외부에서 편집할 수 있는 int List 생성
        //1. foreach문을 통해 List내부 요소들에 접근할 수 있는 반복문 제작!
        //1-1. 각기 접근한 요소들을 출력!
        foreach (int i in int_list)
        {
            Debug.Log(i);
        }
    }

    //>> Dictionary를 직접 생성하여 요소를 추가하고 요소를 불러와보자!
    //int - string Dictionary!
    //1, 2 , 3 ,4 ,5 를 키값으로 넣으면, 해당 숫자의 이름을 Value로 넣는 Dictionary!
    //저 숫자중 하나를 통해 값을 불러와서 출력!
    //없는 숫자중 하나를 통해 값을 불러와서 오류가 나는지 테스트!
    public void DictionaryTest01()
    {
        //0. int - string Dictionary를 생성!
        //1. 1, 2, 3, 4, 5를 키값으로, 해당 키값 숫자의 이름을 Value로 갖도록 Add!
        //2. 그 중 3으로 TryGetValue를 통해 값을 불러와서 출력!
        //3. 10을 통해 TryGetValue를 실행하여,  값을 불러와 지는지 테스트!

        int_name_Dictionary.Add(1, "일");
        int_name_Dictionary.Add(2, "이");
        int_name_Dictionary.Add(3, "삼");
        int_name_Dictionary.Add(4, "사");
        int_name_Dictionary.Add(5, "오");


        string name = "";
        if (int_name_Dictionary.TryGetValue(3, out name))
        {
            Debug.Log(name);
        }
        else
        {
            Debug.Log("해당 키값이 존재하지 않습니다!");
        }

        if (int_name_Dictionary.TryGetValue(10, out name))
        {
            Debug.Log(name);
        }
        else
        {
            Debug.Log("해당 키값이 존재하지 않습니다!");
        }

    }

    //>> foreach를 통하여 딕셔너리 내부 요소에 접근하여 출력해보자!
    //학생의 이름을 키값으로, 해당 학생의 점수를 Value로 갖고있는 Dictionary 생성
    //1. 3명의 학생 이름을 통해 점수를 넣는다!
    //2. 학생 이름과 학생의 점수를 같이 출력!
    public void DictionaryTest2()
    {
        //0. 문자열을 키값으로, 정수형을 Value값으로 갖고있는 Dictionary생성!
        //1. 학생 3명의 이름과 그 학생들의 점수를 Dictionary에 Add!
        //2. Dictionary + foreach!
        //2-1. KeyValuePair를 통해 각 학생들의 이름과 점수를 출력!
                
        studentGradeDictionary.Add("김철수" , 87);
        studentGradeDictionary.Add("채소희" , 78);
        studentGradeDictionary.Add("오창수" , 52);

        foreach(var pair in studentGradeDictionary)
        {
            //print(pair.Key);
            //print(pair.Value);

            string msg;            
            msg = pair.Key + " 학생의 점수는 " + pair.Value + "점 입니다.";
            //msg = $"{pair.Key} 학생의 점수는 {pair.Value} 점 입니다.";

            Debug.Log(msg);


        }

    }
   
    

}
