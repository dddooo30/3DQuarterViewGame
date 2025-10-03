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

        moveVec = new Vector3(hAxis,0,VAxis).normalized; //normalized: 방향값이 1로 보정된 벡터

        transform.position += moveVec * speed * Time.deltaTime; // 현재 위치에 입력받은 좌표값을 더함(입력마다 반복)
    
    }
}
