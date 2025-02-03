using UnityEngine;
using System;

public class PlayerFollowCamera : MonoBehaviour
{
    [Header("Main Camera")][SerializeField] Camera _followerCamera;
    [Header("Target Object")][SerializeField] GameObject _targetObject;

    void Update()
    {
        
    }
}
