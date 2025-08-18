using UnityEngine;

public class ObjectDrop : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 3f;

    private Rigidbody rb;
    private bool hasDropped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // ó���� �߷� ���� X
    }

    void Update()
    {
        if (hasDropped) return;

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist <= triggerDistance)
        {
            rb.isKinematic = false; // �߷� ����
            hasDropped = true;
        }
    }
}