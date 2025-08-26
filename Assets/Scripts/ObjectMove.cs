using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 3f;
    public Vector3 pushDirection = Vector3.right;  // 이동 방향
    public float pushForce = 3f;

    private Rigidbody rb;
    private bool hasPushed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (hasPushed) return;

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist <= triggerDistance)
        {
            rb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
            hasPushed = true;
        }
    }
}
