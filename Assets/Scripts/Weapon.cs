using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{       
    public enum Type { Melee, Range}
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }

    //�ڷ�ƾ ����
    IEnumerator Swing() // ������ �Լ� Ŭ���� yield�� �ݵ�� ������
    {
        //1
        yield return new WaitForSeconds(0.1f);// 0.1�� ��� new null -> 1�����Ӿ� ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        //2 
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        //3
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    //Use() ���� ��ƾ -> Swing() ���� ��ƾ -> Use() ���� ��ƾ 
    //Use() ���� ��ƾ + Swing() �ڷ�ƾ (Co-Op) �ڷ�ƾ ����
}
