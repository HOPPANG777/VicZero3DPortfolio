using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    //public int i { get { return _i; } set { _i = value; } }
    //[SerializeField] private int _i;

    public int i { get { return _i; } }
    [SerializeField] private int _i;

    public float f { get { return _f; } }
    [SerializeField] private float _f;

    public string s { get { return _s; }  }
    [SerializeField] private string _s;

    float t = 0.0f;

    void Start()
    {
        print("Hello, World!");
        print(i);
        print(f);
        print(s);
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
