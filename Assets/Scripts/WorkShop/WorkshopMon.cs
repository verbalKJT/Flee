using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class WorkshopMon : MonoBehaviour
{
  
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

        GameObject player = other.gameObject;

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

        StartCoroutine(TeleportPlayer(player,cc));

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

    IEnumerator TeleportPlayer(GameObject player, CharacterController cc)
    {
        cc.enabled = false;
        yield return null; // 한 프레임 대기
        yield return null; // 두 프레임 대기 (더 안정성 확보)

        player.transform.position = playerPosition.position;

        cc.enabled = true;
        Debug.Log("플레이어 순간이동 완료: " + player.transform.position);
    }
}



