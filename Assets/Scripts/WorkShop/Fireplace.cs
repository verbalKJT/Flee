using UnityEngine;

public class Fireplace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WorkshopMon"))
        {
            
            Debug.Log("몬스터 벽난로 충돌");
            other.gameObject.SetActive(false);
            
        }
       
    }
}
