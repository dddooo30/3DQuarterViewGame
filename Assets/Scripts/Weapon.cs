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

    //코루틴 패턴
    IEnumerator Swing() // 열거형 함수 클래스 yield가 반드시 들어가야함
    {
        //1
        yield return new WaitForSeconds(0.1f);// 0.1초 대기 new null -> 1프레임씩 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        //2 
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        //3
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    //Use() 메인 루틴 -> Swing() 서브 루틴 -> Use() 메인 루틴 
    //Use() 메인 루틴 + Swing() 코루틴 (Co-Op) 코루틴 패턴
}
