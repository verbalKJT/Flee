using UnityEngine;
using UnityEngine.AI;

public class HolywayMonsterAI : MonoBehaviour
{
    [Header("AI 기본 설정")]
    public float detectRange = 10f;          // 플레이어 탐지 범위
    public float chaseRange = 6f;            // 추격 시작 거리
    public float stopDistance = 1.8f;        // 플레이어를 붙잡는 거리
    [Range(10f, 180f)]
    public float fieldOfView = 120f;         // 시야각
    public LayerMask obstacleMask;           // 시야 가림 체크용 레이어

    [Header("컴포넌트")]
    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private bool hasCaughtPlayer = false;
    private bool isChasing = false;
    private bool hasSpottedPlayer = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Player 자동 탐색
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("🎯 [HolywayMonsterAI] Player 자동 연결 완료");
        }
        else
        {
            Debug.LogWarning("⚠️ [HolywayMonsterAI] Player를 찾을 수 없습니다!");
        }
    }

    private void Update()
    {
        if (player == null || hasCaughtPlayer)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 시야각 내에 있는지 확인
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle < fieldOfView * 0.5f && distance <= detectRange)
        {
            // 시야에 들어오면 Raycast로 시야막힘 확인
            if (!Physics.Linecast(transform.position + Vector3.up * 1.2f, player.position + Vector3.up * 1.2f, obstacleMask))
            {
                hasSpottedPlayer = true;
            }
        }

        if (hasSpottedPlayer)
        {
            ChasePlayer(distance);
        }
        else
        {
            Patrol();
        }
    }

    private void ChasePlayer(float distance)
    {
        if (distance > chaseRange)
        {
            // 너무 멀어지면 추격 중단
            hasSpottedPlayer = false;
            isChasing = false;
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (distance > stopDistance)
        {
            // 추격 중
            isChasing = true;
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            // 붙잡음
            CatchPlayer();
        }
    }

    private void Patrol()
    {
        // 대기 또는 기본 Idle 애니메이션
        animator.SetFloat("Speed", 0f);
    }

    /// <summary>
    /// 플레이어를 붙잡았을 때 실행되는 함수
    /// </summary>
    private void CatchPlayer()
    {
        if (hasCaughtPlayer)
            return;

        hasCaughtPlayer = true;
        agent.isStopped = true;
        animator.SetFloat("Speed", 0f);

        Debug.Log("💀 [HolywayMonsterAI] 플레이어를 붙잡음 - 컷씬 요청");

        // ✅ DeadGameoverHolyway 스크립트 찾아서 컷씬 실행
        DeadGameoverHolyway deathCutscene = FindObjectOfType<DeadGameoverHolyway>();
        if (deathCutscene != null)
        {
            deathCutscene.PlayDeathCutscene(this);
        }
        else
        {
            Debug.LogWarning("⚠️ [HolywayMonsterAI] DeadGameoverHolyway를 찾지 못했습니다!");
        }
    }

    /// <summary>
    /// 컷씬 종료 후 제거용 함수 (DeadGameoverHolyway에서 호출 가능)
    /// </summary>
    public void DespawnAfterCutscene()
    {
        Debug.Log("🧩 [HolywayMonsterAI] 컷씬 종료 후 몬스터 제거됨");
        Destroy(gameObject);
    }
}
