using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public int hasGrenades;
    public Camera followCamer;

    public int ammo;
    public int coin;
    public int health;
    
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    int equipWeaponsIndex = -1;
    float hAxis;
    float vAxis;
    float fireDelay;
    
    bool wDown;
    bool jDown;
    bool fDown;
    bool rDown;

    bool isjump;
    bool isdodge;
    bool iDown;
    bool isSwap;
    bool isFireReady = true;
    bool isReload;
    bool isBorder;

    bool sDown1;
    bool sDown2;
    bool sDown3;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Animator anim;
    Rigidbody rb;

    GameObject nearObject;
    Weapon equipWeapon;

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
        Turn();
        Jump();
        Attack();
        Reload();
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
        fDown = Input.GetButton("Fire1");
        iDown = Input.GetButtonDown("interaction");
        rDown = Input.GetButtonDown("Reload");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //normalized: 방향값이 1로 보정된 벡터 

        if (isdodge) moveVec = dodgeVec;
        
        if (isSwap || !isFireReady || isReload) moveVec = Vector3.zero;
        
        if (!isBorder) {
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; // 삼항연산자 yes면 앞에 값 아니면 뒤에 값
        }
        anim.SetBool("isrun", moveVec != Vector3.zero);
        anim.SetBool("iswalk", wDown);
    }

    void Turn()
    {
        //키보드에 의한 회전 //바라보는 시선
        transform.LookAt(transform.position + moveVec);
        //마우스에 의한 회전
        if (fDown) {
            Ray ray = followCamer.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
        
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

    void Attack()
    {
        if (equipWeapon == null) return;
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isdodge && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }

    void Reload()
    {
        if (equipWeapon == null) return; 
        if (equipWeapon.type == Weapon.Type.Melee) return;
        if (ammo == 0) return;
        
        if (rDown && !isjump && !isdodge && !isSwap && isFireReady)
        {
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 3f);
        }
    } 

    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
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
            if(equipWeapon != null) equipWeapon.gameObject.SetActive(false);
            equipWeaponsIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

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

    void FreezeRotation()
    {
        rb.angularVelocity = Vector3.zero;
    }

    void StoptoWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        FreezeRotation();
        StoptoWall();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isjump", false);
            isjump = false;
        }   
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type) 
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo) ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin) coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth) health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if(hasGrenades > maxHasGrenades) hasGrenades = maxHasGrenades;
                    break;
            }
            Destroy(other.gameObject);  
        }

        
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = other.gameObject;
        if (other.tag != "Weapon")
            nearObject = other.gameObject;

        Debug.Log(nearObject.name);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }
}
