using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Follwer : MonoBehaviour
{
    public Transform target; //ī�޶� ���� ������Ʈ
    public Vector3 offset; //������

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
