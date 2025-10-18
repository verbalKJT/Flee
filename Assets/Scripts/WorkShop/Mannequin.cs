using UnityEngine;

public class Mannequin : MonoBehaviour
{
    [Header("감지설정")]
    public Transform player; //플레이어 Transform
    public float lookRange = 5f; //감지거리

    void Update()
    {
        //플레이어 null예외처리
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 플레이어가 일정 거리 안으로 들어오면 반응
        /*if (distance < lookRange && !hasTriggered)
        {
            TriggerReaction();
        }*/

        // 시선 추적 (지속적으로)
        if ( distance < lookRange * 2)
        {
            Vector3 dir = player.position - transform.position;
            dir.y = 0; // 수평 회전만
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 2f);
        }
    }
}
