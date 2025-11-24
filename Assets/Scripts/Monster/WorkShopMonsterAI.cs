using UnityEngine;
using UnityEngine.AI;

public class WorkShopMonsterAI : MonoBehaviour
{

    private Transform target;
    private NavMeshAgent agent;

    //커브구간 진입 트리거
    private bool inCurveZone = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // Rigidbody가 있으면 NavMeshAgent와 충돌함 → kinematic 처리
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void Start()
    {
        // 자동으로 플레이어 찾기 (Tag 사용)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player 태그를 가진 오브젝트가 없습니다!");
        }
    }

    private void Update()
    {
        if (target == null || agent == null)
            return;

        if (!agent.isOnNavMesh)
            return;

        agent.SetDestination(target.position);
    }

    public void EnterCurveZone()
    {
        inCurveZone = true;
        Debug.Log(inCurveZone);
        // 코너 들어오면 회전속도 크게 낮추기
        agent.angularSpeed = 30f;
        // 직진 가속도 증가
        agent.acceleration = 80f;
        // 속도 증가로 미끄러지는 느낌
        agent.speed += 2f;

        Debug.Log("몬스터 커브 진입 : 회전 저하 + 가속 증가");
    }
}
