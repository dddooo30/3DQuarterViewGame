using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rb;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, 
                            Vector3.up, 0f, LayerMask.GetMask("Enemy"));

         
        foreach(RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enenmy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 5);
    }
}
