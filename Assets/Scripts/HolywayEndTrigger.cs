using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HolywayEndTrigger : MonoBehaviour
{
    private holyway owner;

    void Awake()
    {
        owner = GetComponentInParent<holyway>();
        var col = GetComponent<Collider>();
        col.isTrigger = true; // ¹Ýµå½Ã Trigger
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        EndCorridorGameManager.I?.OnEndReached(owner);
    }
}
