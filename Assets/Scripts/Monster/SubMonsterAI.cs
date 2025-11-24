using UnityEngine;
using UnityEngine.AI;

public class SubMonsterAI : Monster
{
    public float chaseRange = 10f;
    public float stopDistance = 1.5f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool hasCaughtPlayer = false;

    private void Update()
    {
        // NavMeshAgent가 꺼졌거나 NavMesh 위가 아니면 즉시 종료 (오류 방지)
        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            return;

        if (animator == null)
            return;

        if (hasCaughtPlayer)
        {
            agent.isStopped = true;
            agent.ResetPath();
            animator.SetFloat("Speed", 0);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetFloat("Speed", agent.velocity.magnitude);

            if (distance <= stopDistance)
            {
                hasCaughtPlayer = true;

                var col = GetComponent<Collider>();
                Physics.IgnoreCollision(col, player.GetComponent<Collider>(), false);

                col.enabled = false;
                col.enabled = true;
            }
        }
        else
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0);
        }
    }


    public override void OnPlayerSetupComplete()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
