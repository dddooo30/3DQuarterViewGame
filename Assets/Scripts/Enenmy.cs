using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enenmy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public Transform target;
    public bool isChase;

    Rigidbody rb;
    BoxCollider boxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2);
    }

    void Update()
    {
        if(isChase) nav.SetDestination(target.position);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void FixedUpdate()
    {
        FreezeVelcocity();
    }

    void FreezeVelcocity()
    {
        if (isChase)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }   
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, false));

            Debug.Log("Melee : " + curHealth);
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth-= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, false));
            Destroy(other.gameObject);

            Debug.Log("Range : " + curHealth);
        }
        
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            mat.color = Color.white;
        }
        else {

            mat.color = Color.gray;
            gameObject.layer = 12;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");
            if (isGrenade) 
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                rb.freezeRotation = false;
                rb.AddForce(reactVec * 5, ForceMode.Impulse);
                rb.AddTorque(reactVec * 15, ForceMode.Impulse);
            } 
            else
            { 
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rb.AddForce(reactVec * 5, ForceMode.Impulse);
            }
                
            Destroy(gameObject, 4);
        }
    }
}
