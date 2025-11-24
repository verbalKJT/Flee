using UnityEngine;

public class Fireplace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WorkshopMon"))
        {
            
            Debug.Log("몬스터 벽난로 충돌");
            //몬스터 비활성화
            other.gameObject.SetActive(false);
            //기믹 수행 체크
            Inventory.Instance.MonInFireplace=true;
            
        }
       
    }
}
