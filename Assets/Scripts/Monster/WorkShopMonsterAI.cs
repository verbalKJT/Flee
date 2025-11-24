using UnityEngine;
using UnityEngine.AI;

public class WorkShopMonsterAI : MonoBehaviour
{
    
    public Transform target;
    private NavMeshAgent agent;
    private Rigidbody rb;

    [Header("가속도 설정")] 
    public float acceleration = 15f;
    public float maxSpeed = 5f; 
    public float rotationSpeed = 5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.updatePosition = false; // Rigidbody로 이동 제어
        agent.updateRotation = false; // 수동 회전
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 회전 고정
    }

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void FixedUpdate()
    {
        if (agent.pathPending || agent.remainingDistance <= agent.stoppingDistance) return;

        Vector3 desiredVelocity = agent.desiredVelocity;
        Vector3 steering = desiredVelocity.normalized * maxSpeed - rb.linearVelocity;
        steering = Vector3.ClampMagnitude(steering, acceleration * Time.fixedDeltaTime);

        rb.linearVelocity += steering;

        //회전 부드럽게
        if (rb.linearVelocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }
    }
}
