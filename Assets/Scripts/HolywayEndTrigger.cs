using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HolywayEndTrigger : MonoBehaviour
{
    private holyway owner;

    void Awake()
    {
        owner = GetComponentInParent<holyway>();
        var col = GetComponent<Collider>();
        col.isTrigger = true; // 반드시 Trigger 이거 안하면 물체 그냥 지나가질수도있음
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        EndCorridorGameManager.I?.OnEndReached(owner);
    }
}
