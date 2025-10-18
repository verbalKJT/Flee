using UnityEngine;
using UnityEngine.AI;

public class HolywayMonsterAI : Monster
{
    [Header("AI Settings")]
    public float chaseRange = 2f;        // 감지 거리
    public float stopDistance = 1.5f;     // 최소 거리
    [Range(10f, 180f)]
    public float fieldOfView = 5f;
    public LayerMask obstacleMask;        // 시야를 막는 오브젝트 레이어 지정용

    private NavMeshAgent agent;
    private Animator animator;
    private bool hasCaughtPlayer = false;
    private bool isChasing = false;
    private bool hasSpottedPlayer = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        agent.isStopped = true;
    }

    void Update()
    {
        if (agent == null || animator == null || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // 시야각과 거리 조건 확인
        if (distance <= chaseRange && angleToPlayer <= fieldOfView / 2)
        {
            // 물체에 막히지 않은 경우만 본 걸로 처리
            if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, directionToPlayer, distance, obstacleMask))
            {
                hasSpottedPlayer = true;
            }
        }

        // 플레이어를 봤거나 이미 추격 중이라면 계속 추격
        if (hasSpottedPlayer && !hasCaughtPlayer)
        {
            if (!isChasing)
            {
                isChasing = true;
                agent.isStopped = false;
                animator.SetFloat("Speed", 1f);
            }

            agent.SetDestination(player.position);

            // 부드러운 회전
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            float speed = agent.velocity.magnitude;
            if (speed > 0.05f)
                animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), speed, Time.deltaTime * 5f));

            if (distance <= stopDistance)
            {
                hasCaughtPlayer = true;
                agent.isStopped = true;
                animator.SetFloat("Speed", 0f);

                // ✅ 플레이어 잡는 순간 바로 몬스터 제거
                Destroy(gameObject);
            }
        }
        else
        {
            // 플레이어 못봤을 때만 Idle 유지
            if (!hasSpottedPlayer)
            {
                if (isChasing)
                {
                    isChasing = false;
                    agent.isStopped = true;
                    agent.ResetPath();
                }
                animator.SetFloat("Speed", 0f);
            }
        }
    }

    public override void OnPlayerSetupComplete()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

#if UNITY_EDITOR
    // Scene 뷰에서 시야 범위 시각화
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * chaseRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * chaseRange);
    }
#endif
}
