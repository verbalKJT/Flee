using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CrawlMon"))
        {
            Debug.Log("몬스터 Destroy Zone 트리거 진입 → 삭제!");

            Destroy(other.gameObject);
        }
    }
}
