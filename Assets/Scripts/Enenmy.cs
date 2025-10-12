using System.Collections;
using UnityEngine;

public class Enenmy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;

    Rigidbody rb;
    BoxCollider boxCollider;
    Material mat;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponent<MeshRenderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));

            Debug.Log("Melee : " + curHealth);
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth-= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
            Destroy(other.gameObject);

            Debug.Log("Range : " + curHealth);
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
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

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;

            rb.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 4);
        }
    }
}
