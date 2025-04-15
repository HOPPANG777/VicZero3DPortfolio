using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderStudy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //>> 하나의 스크립트를 만들어서, 큐브에 컴포넌트화 시킨 후, 해당 콜리전 이벤트 함수들을 호출시켜보자!

    void OnCollisionEnter(Collision collision)
    {
        print("OnCollisionEnter 호출! " + collision.collider.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        print("OnCollisionStay 호출! " + collision.collider.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        print("OnCollisionExit 호출! " + collision.collider.name);
    }

    //>> 하나의 스크립트를 만들어서, 큐브에 컴포넌트화 시킨 후, 해당 트리거 이벤트 함수들을 호출시켜보자!

    private void OnTriggerEnter(Collider other)
    {
        print("OnTriggerEnter 호출! " + other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        print("OnTriggerStay 호출! " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        print("OnTriggerExit 호출! " + other.name);
    }

}
