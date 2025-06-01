using Script;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform player; // 플레이어 Transform 참조
    [SerializeField] private float sightRange; // 인식 거리
    [SerializeField] private float sightAngle; // 시야각
    [SerializeField] private float attackRange; // 공격 범위  


    public Transform[] patrolPoints; // 순찰할 지점들의 배열
    private int currentPatrolIndex = 0; // 인덱스

    private EnemyState currentState = EnemyState.PATROL; // 적의 현재 상태
    private NavMeshAgent agent; // 이동 제어

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = 3.5f;
        Debug.Log("NavMesh 위에 있음?: " + agent.isOnNavMesh); // NavMesh 상태 확인
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.PATROL:
                Patrol();
                animator.SetFloat("Action", 0.5f);
                break;
            case EnemyState.CHASE:
                ChasePlayer();
                animator.SetFloat("Action", 1f);
                break;
            case EnemyState.ATTACK:
                AttackPlayer();
                animator.SetFloat("Action", 0f);
                break;
        }
    }

    void Patrol()
    {
        // 현재 목적지에 거의 도착 + 경로가 아직 계산 중이 아니면
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            // 다음 목적지 무작위 설정
            currentPatrolIndex = Random.Range(0, patrolPoints.Length);
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

        // 탐지
        LookForPlayer();
    }

    void LookForPlayer()
    {
        if (player == null) return;
        
        // 플레이어 위치 - 적 위치 방향 벡터 계산
        Vector3 dirToPlayer = player.position - transform.position;

        // 수평 시야각 계산
        float horizontalAngle = Vector3.Angle(transform.forward, dirToPlayer);

        // 시야 범위와 시야각 조건을 만족할 경우
        if (dirToPlayer.magnitude < sightRange && horizontalAngle < sightAngle / 2f)
        {
            // 레이 시작 위치: 적 캐릭터의 가슴 높이에서 시작
            Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;

            // 레이 목표 위치: 플레이어의 가슴 또는 머리 높이
            Vector3 targetPoint = player.position + Vector3.up * 0.9f;

            // 레이 방향 계산
            Vector3 direction = (targetPoint - rayOrigin).normalized;

            // 레이 생성
            Ray ray = new Ray(rayOrigin, direction);

            // 디버그용 레이 시각화 (씬 뷰에서 빨간 선으로 확인 가능)
            Debug.DrawRay(rayOrigin, direction * sightRange, Color.red);

            // 레이캐스트 발사
            if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
            {
                Debug.Log("Raycast hit: " + hit.transform.name); // 어떤 오브젝트를 맞췄는지 출력

                // 맞춘 대상이 플레이어일 경우
                if (hit.transform == player)
                {
                    Debug.Log("플레이어 발견! 추격 시작");
                    currentState = EnemyState.CHASE; // 상태를 추격으로 전환
                }
            }
        }
    }

    void ChasePlayer()
    {
        //플레이어가 숨는 상태를 받아올 변수 선언
        var hider = player.GetComponent<PlayerHider>();
        //플레이어가 숨는 중이면 탐지 안되도록
        if (hider != null && hider.IsHiding) currentState = EnemyState.PATROL; 
        
        // 목적지 플레이어 위치로
        agent.SetDestination(player.position);
        // 달리기
        agent.speed = 5f;
        
        // 플레이어 사이의 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);

        // 공격범위 내라면
        if (distance <= attackRange)
        {
            // 공격
            currentState = EnemyState.ATTACK;
        }
        else if (distance > sightRange * 2f) // 너무 멀어지면 다시 Patrol
        {
            currentState = EnemyState.PATROL;
        }
    }

    // 공격
    void AttackPlayer()
    {
        // 플레이어 사이의 거리
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            // 벗어나면 추격
            currentState = EnemyState.CHASE;
        }
    }
}