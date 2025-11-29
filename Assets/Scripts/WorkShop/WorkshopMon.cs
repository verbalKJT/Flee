using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class WorkshopMon : MonoBehaviour
{
    [Header("플레이어")]
    public GameObject player;

    
    private Transform playerPosition;
    private Transform monsterPosition;

    private void Start()
    {
        // 싱글톤에서 정보 받아옴        
        playerPosition = WorkShopManager.Instance.playerResetPoint;
        monsterPosition = WorkShopManager.Instance.monsterResetPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[WorkshopMon] 충돌 감지됨: {other.name}");

        if (player == null)
        {
            Debug.LogError("player가 null입니다.");
            return;
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc == null)
        {
            Debug.LogError("CharacterController 못 찾음");
            return;
        }

        StartCoroutine(TeleportPlayer(cc));

        // 몬스터 순간이동
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                transform.position = monsterPosition.position;
                agent.enabled = true;

                // 다시 플레이어 쫓기 시작
                agent.SetDestination(player.transform.position);
            }
        }

    IEnumerator TeleportPlayer(CharacterController cc)
    {
        cc.enabled = false;
        Debug.Log(cc.enabled);
        yield return null;  // 한 프레임 쉬기
        yield return null;  // 한 프레임 쉬기
        player.transform.position = playerPosition.position;
        Debug.Log(player.transform.position);
        cc.enabled = true;
        Debug.Log(cc.enabled);

        Debug.Log("플레이어 순간이동 완료");
    }

}



