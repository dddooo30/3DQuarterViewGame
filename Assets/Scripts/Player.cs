using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float VAxis;

    Vector3 moveVec;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        VAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis,0,VAxis).normalized; //normalized: ���Ⱚ�� 1�� ������ ����

        transform.position += moveVec * speed * Time.deltaTime; // ���� ��ġ�� �Է¹��� ��ǥ���� ����(�Է¸��� �ݺ�)
    
    }
}
