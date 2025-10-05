using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Follwer : MonoBehaviour
{
    public Transform target; //카메라가 따라갈 오브젝트
    public Vector3 offset; //보정값

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
