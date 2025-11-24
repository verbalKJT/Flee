using UnityEngine;

public class Fireplace : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("WorkshopMon"))
        {
            Debug.Log("몬스터 벽난로 충돌");
        }
       
    }
}
