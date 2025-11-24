using UnityEngine;
using UnityEngine.AI;

public class WorkShopMonsterAI : MonoBehaviour
{

    private Transform target;
    private NavMeshAgent agent;

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
}
