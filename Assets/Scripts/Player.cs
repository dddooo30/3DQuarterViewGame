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
        anim = GetComponentInChildren<Animator>(); // animator ������Ʈ�� �ڽ����� �־��� ������ 
    }                                              // GetComponentInChildren ���

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis,0,vAxis).normalized; //normalized: ���Ⱚ�� 1�� ������ ���� 
        transform.position += moveVec * speed* (wDown ? 0.3f : 1f) * Time.deltaTime; // ���׿����� yes�� �տ� �� �ƴϸ� �ڿ� ��

        anim.SetBool("isrun", moveVec != Vector3.zero);
        anim.SetBool("iswalk", wDown);

        transform.LookAt(transform.position + moveVec);
    }
}
