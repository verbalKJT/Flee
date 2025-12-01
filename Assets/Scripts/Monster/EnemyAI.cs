using Script;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Monster
{
    private Animator animator;
    // 플레이어 Transform 참조
    [SerializeField] private float sightRange; // 인식 거리
    [SerializeField] private float sightAngle; // 시야각
    [SerializeField] private float attackRange; // 공격 범위  


    public Transform[] patrolPoints; // 순찰할 지점들의 배열
    private int currentPatrolIndex = 0; // 인덱스

    private EnemyState currentState = EnemyState.PATROL; // 적의 현재 상태
    private NavMeshAgent agent; // 이동 제어

    [Header("손전등 기절 설정")]
    public bool canStun = false;    //미들 몬스터 프리펩에만 True로 설정
    public float stunDuration = 3f; //기절 시간
    public float stunCooltime = 5f; //기절 끝난 뒤 추가 쿨타임
    private bool isStunned = false; //기절 상태
    private float stunEndTime = 0f;
    private float nextStunTime = 0f;    //이 시간이 지나야 다시 스턴 가능
    [SerializeField] private string stunTriggerName = "Stun";
    
    [Header("분노 상태 설정")]
    public float angryDuration = 5f;
    public float angrySpeedMultiplier = 1.5f;
    private float angryEndTime = 0f;
    private bool isAngry = false;
    
    [Header("메인 몬스터 설정")]
    private bool isMain = false;
    private const int essential_PointIndex = 4;
    private int previousPatrolIndex = 0; // 이전 패트롤 위치
    
    void Update()
    {
        // 할당되는 시간 
        if (agent == null || animator == null)
        {
            return;
        }

        //기절 상태 처리
        if (isStunned)
        {
            if (Time.time >= stunEndTime)
            {
                isStunned = false;
                if (agent != null)
                    agent.isStopped = false;

                changeState(EnemyState.PATROL);
            }

            return;
        }
        
        // 분노
        if (isAngry && Time.time >= angryEndTime)
        {
            isAngry = false;
    
            // 속도 원래대로 복구
            agent.speed /= angrySpeedMultiplier;

            // 상태 복귀
            changeState(EnemyState.PATROL);

            
        }

        switch (currentState)
        {
            case EnemyState.PATROL:
                Patrol();
                animator.SetFloat("Action", 0.5f);
                //animator.speed = agent.velocity.magnitude / agent.speed * 1.2f;
                animator.speed = 1f; //애니메이션 재생 속도를 애니메이터 설정대로 재생.
                animator.SetBool("isRunning", false);
                break;
            case EnemyState.CHASE:
                ChasePlayer();
                //animator.speed = agent.velocity.magnitude / agent.speed * 1.2f;
                animator.speed = 1f; //애니메이션 재생 속도를 애니메이터 설정대로 재생.
                animator.SetFloat("Action", 1f);
                animator.SetBool("isRunning", true);
                break;
            case EnemyState.STUN:
                animator.SetBool("isRunning", false);
                animator.speed = 1f;
                break;
            case EnemyState.ANGRY:
                //animator.speed = agent.velocity.magnitude / agent.speed * 1.8f;
                animator.speed = 1f; //애니메이션 재생 속도를 애니메이터 설정대로 재생.
                animator.SetFloat("Action", 1f);
                animator.SetBool("isRunning", true);
                break;
        }
    }

    private void Patrol()
    {
        // 현재 목적지에 거의 도착 + 경로가 아직 계산 중이 아니면
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            int nextPoint = Random.Range(0, patrolPoints.Length);

            if (isMain){ 
                    // 현재 위치가 '문' 인경우
                if (currentPatrolIndex == essential_PointIndex)
                {
                    // 이전이 낮은 구역(0~3) -> 높은 구역(5~)으로 보냄
                    if (previousPatrolIndex < essential_PointIndex)
                    {
                        nextPoint = Random.Range(essential_PointIndex + 1, patrolPoints.Length);
                    }
                    // 이전이 높은 구역(5~) -> 낮은 구역(0~3)으로 보냄
                    else
                    {
                        nextPoint = Random.Range(0, essential_PointIndex);
                    }
                }
                // 위치가 일반
                else
                {
                    nextPoint = Random.Range(0, patrolPoints.Length); // 일단 랜덤

                    // 맵 크기가 충분하다면 가로지르기 검사
                    if (patrolPoints.Length > essential_PointIndex)
                    {
                        bool up = currentPatrolIndex < essential_PointIndex && nextPoint > essential_PointIndex;
                        bool down = currentPatrolIndex > essential_PointIndex && nextPoint < essential_PointIndex;

                        // 선을 넘으려고 하면 4번(문)으로 강제 변경
                        if (up || down)
                        {
                            nextPoint = essential_PointIndex;
                        }
                    }
                }
            }
            else 
            {
                // 미들 몬은아무 조건 없이 그냥 무작위 이동
                nextPoint = Random.Range(0, patrolPoints.Length);
            }
            
        
            previousPatrolIndex = currentPatrolIndex; // 현재 위치 저장
            currentPatrolIndex = nextPoint;           // 목적지 갱신
        
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        // 탐지
        LookForPlayer();
    }

    private void LookForPlayer()
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
                // 맞춘 대상이 플레이어일 경우
                if (hit.transform == player)
                {
                    changeState(EnemyState.CHASE); // 상태를 추격으로 전환
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
        agent.speed = 7f;

        // 플레이어 사이의 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > sightRange * 2f) // 너무 멀어지면 다시 Patrol
        {
            changeState(EnemyState.PATROL);
        }
    }
    // 상태 변화 매소드로 분리
    private void changeState(EnemyState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case EnemyState.PATROL:
                animator.SetFloat("Action", 0.5f);
                //animator.speed = agent.velocity.magnitude / agent.speed * 1.5f;
                break;
            case EnemyState.CHASE:
                //animator.speed = agent.velocity.magnitude / agent.speed * 1.8f;
                animator.SetFloat("Action", 1f);
                break;
            case EnemyState.STUN:
                animator.SetFloat("Action", 0f);
                animator.speed = 1f;
                break;
            case EnemyState.ANGRY:
                //animator.speed = agent.velocity.magnitude / agent.speed * 2f;
                animator.SetFloat("Action", 1f);
                break;
        }
    }

    public override void OnPlayerSetupComplete()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = 4f;

        if (gameObject.name == "MainMon")
        {
            isMain = true;
            agent.speed = 3.5f;
        }
    }
    
    // 오브젝트 맞았을 때 분노 함수
    public void TriggerAnger()
    {
        if (!isAngry)
        {
            isAngry = true;
            angryEndTime = Time.time + angryDuration;

            // 속도 증가
            agent.speed *= angrySpeedMultiplier;

            // 상태 변경
            changeState(EnemyState.ANGRY);

            Debug.Log($"{gameObject.name} 이 분노 상태에 진입했습니다!");
        }
    }
    
    // 손전등에서 호출할 함수
    public void StunByFlashlight()
    {
        if (!canStun) return;   // 인스펙터 CanStun 옵션에 따라 스턴 여부 판정

        if (Time.time < nextStunTime) return;   //쿨타임 지나야 스턴 되도록 설정
        if (isStunned) return;  // 이미 기절 중이면 또 안 걸림

        isStunned = true;
        stunEndTime = Time.time + stunDuration;

        nextStunTime = Time.time + stunDuration + stunCooltime;

        if (animator != null && !string.IsNullOrEmpty(stunTriggerName))
        {
            animator.SetTrigger(stunTriggerName);
        }

        // 상태를 STUN 으로 전환
        changeState(EnemyState.STUN);

        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }
}