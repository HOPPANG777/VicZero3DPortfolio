using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    //public int i { get { return _i; } set { _i = value; } }
    //[SerializeField] private int _i;

    public int i { get { return _i; } }
    [SerializeField] private int _i;

    public float f { get { return _f; } }
    [SerializeField] private float _f;

    public string s { get { return _s; } }
    [SerializeField] private string _s;

    float t = 0.0f;
    uint count = 0;

    void Start()
    {
        print("Hello, World!");
        print(i);
        print(f);
        print(s);

        print(5 == 5);
        print(!(5 > 3));
        print(!true);

        while (count < 5)
        {
            print("이게 몇번째냐");

            count++;
        }

        for (int i = 0; i < 5; i++)
        {
            print("인생이 힘들다고 생각할때 깨달아야한다 우리 부모님이 우리를 위해 얼마나 열심히 일하시는지.");   
        }

        int num3 = 1;

        do
        {
            print(num3);
            num3++;
        } while (num3 <= 100);
    }

    void Update()
    {
        t += 1 * Time.deltaTime;
       
        if (t >= 2)
        {
            print(s);
            t = 0.0f;
        }
            
    }
}
