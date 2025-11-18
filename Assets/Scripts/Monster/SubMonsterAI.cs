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
        if (agent == null || animator == null) return;

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

                // ⭐ 몬스터 collider를 강제로 플레이어와 충돌시켜 Timeline 발동
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
