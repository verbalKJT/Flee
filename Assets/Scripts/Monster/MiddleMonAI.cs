using UnityEngine;
using UnityEngine.AI;

public class MiddleMonAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isReactingToSound = false;

    [Header("반응 설정")]
    public float hearingCooldown = 3f;   // 소리 반응 쿨다운
    private float lastHeardTime = -999f;

    void OnEnable()
    {
        SoundManager.OnSoundEmitted += OnSoundHeard;
    }

    void OnDisable()
    {
        SoundManager.OnSoundEmitted -= OnSoundHeard;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnSoundHeard(Vector3 soundPos, float soundRange)
    {
        if (Time.time - lastHeardTime < hearingCooldown)
            return; // 너무 자주 반응하지 않도록 쿨타임

        float dist = Vector3.Distance(transform.position, soundPos);
        if (dist <= soundRange)
        {
            Debug.Log("[MiddleMon] 소리를 들음! 위치로 이동 중...");
            lastHeardTime = Time.time;
            isReactingToSound = true;

            agent.SetDestination(soundPos);
        }
    }

    void Update()
    {
        // 목적지에 도착하면 반응 종료
        if (isReactingToSound && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isReactingToSound = false;
            Debug.Log("[MiddleMon] 소리 위치 도착, 대기 상태");
            // 여기에 다시 Patrol 시작 등도 추가 가능
        }
    }
}