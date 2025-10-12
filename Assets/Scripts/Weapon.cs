using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{       
    public enum Type { Melee, Range}
    public Type type;

    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type == Type.Range && curAmmo > 0) {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    //�ڷ�ƾ ����
    IEnumerator Swing() // ������ �Լ� Ŭ���� yield�� �ݵ�� ������
    {
        //1
        yield return new WaitForSeconds(0.4f);// 0.1�� ��� new null -> 1�����Ӿ� ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        //2 
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        //3
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        //1.�Ѿ� �߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRig = intantBullet.GetComponent<Rigidbody>();
        bulletRig.linearVelocity = bulletPos.forward * 50;
        yield return null;

        //2. ź�� ����
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRig = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRig.AddForce(caseVec, ForceMode.Impulse);
        caseRig.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

    //Use() ���� ��ƾ -> Swing() ���� ��ƾ -> Use() ���� ��ƾ 
    //Use() ���� ��ƾ + Swing() �ڷ�ƾ (Co-Op) �ڷ�ƾ ����
}
