using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;

    int equipWeaponsIndex = -1;
    float hAxis;
    float vAxis;
    
    bool wDown;
    bool jDown;
    bool isjump;
    bool isdodge;
    bool iDown;
    bool isSwap;
    bool sDown1;
    bool sDown2;
    bool sDown3;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Animator anim;
    Rigidbody rb;

    GameObject nearObject;
    GameObject equipWeapon;

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
        Swap();
        interaction();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("interaction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //normalized: 방향값이 1로 보정된 벡터 

        if (isdodge) moveVec = dodgeVec;
        if (isSwap) moveVec = Vector3.zero;
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; // 삼항연산자 yes면 앞에 값 아니면 뒤에 값

        anim.SetBool("isrun", moveVec != Vector3.zero);
        anim.SetBool("iswalk", wDown);
    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero && !isjump && !isdodge && !isSwap)
        {
            rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isjump", true);
            anim.SetTrigger("dojump");
            isjump = true;
        }
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isjump && !isSwap)
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

    void SwapOut()
    {
        isSwap = false;
    }

    void Swap()
    {
        if(sDown1 && (!hasWeapons[0] || equipWeaponsIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponsIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponsIndex == 2))
            return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && !isjump && !isdodge )
        {
            if(equipWeapon != null) equipWeapon.SetActive(false);
            equipWeaponsIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);

            anim.SetTrigger("doswap");
            isSwap = true;
            Invoke("SwapOut", 0.4f);
        }
    }

    void interaction()
    {
        if (iDown && nearObject != null && !isjump && !isdodge)
            if(nearObject.tag == "Weapon") {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
           
                Destroy(nearObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isjump", false);
            isjump = false;
        }   
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
            nearObject = other.gameObject;

        Debug.Log(nearObject.name);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }
}
