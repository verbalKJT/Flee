using UnityEngine;

public class CurveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WorkshopMon"))
        {
            Debug.Log("CurveTrigger통과1");
            var ai = other.GetComponent<WorkShopMonsterAI>();
            if (ai != null)
            {
                Debug.Log("CurveTrigger통과2");
                ai.EnterCurveZone();
            }
        }
    }
}
