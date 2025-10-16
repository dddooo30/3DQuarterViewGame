using UnityEngine;

public class StartZone : MonoBehaviour
{
    public GameManager Manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            Manager.StageStart();
    }
}
