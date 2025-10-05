using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    bool wDown;

    Vector3 moveVec;

    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>(); // animator 오브젝트를 자식으로 넣었기 때문에 
    }                                              // GetComponentInChildren 사용

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis,0,vAxis).normalized; //normalized: 방향값이 1로 보정된 벡터 
        transform.position += moveVec * speed* (wDown ? 0.3f : 1f) * Time.deltaTime; // 삼항연산자 yes면 앞에 값 아니면 뒤에 값

        anim.SetBool("isrun", moveVec != Vector3.zero);
        anim.SetBool("iswalk", wDown);

        transform.LookAt(transform.position + moveVec);
    }
}
