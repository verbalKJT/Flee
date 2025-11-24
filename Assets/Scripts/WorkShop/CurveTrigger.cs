using UnityEngine;

public class CurveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WorkshopMon"))
        {
            var ai = other.GetComponent<WorkShopMonsterAI>();
            if (ai != null)
            {
                ai.EnterCurveZone();
            }
        }
    }
}
