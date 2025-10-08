using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    
    bool wDown;
    bool jDown;
    bool isjump;
    bool isdodge;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Animator anim;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>(); // animator 오브젝트를 자식으로 넣었기 때문에 
    }                                              // GetComponentInChildren 사용

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        transform.LookAt(transform.position + moveVec); //시선
        Jump();
        Dodge();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //normalized: 방향값이 1로 보정된 벡터 

        if (isdodge) moveVec = dodgeVec;
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; // 삼항연산자 yes면 앞에 값 아니면 뒤에 값

        anim.SetBool("isrun", moveVec != Vector3.zero);
        anim.SetBool("iswalk", wDown);
    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero && !isjump && !isdodge)
        {
            rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isjump", true);
            anim.SetTrigger("dojump");
            isjump = true;
        }
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isjump)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("dododge");
            isdodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isdodge = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isjump", false);
            isjump = false;
        }   
    }
}
