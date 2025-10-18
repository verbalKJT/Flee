using UnityEngine;

public class Mannequin : MonoBehaviour
{
    [Header("플레이어 오브젝트")]
    public Transform player;

    void Update()
    {
        if (player == null) return;

        // 플레이어와의 방향 계산 (수평 회전만)
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {            
            Quaternion target = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 0f, 0f);
            transform.rotation = target;
        }
    
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
        Debug.Log("플레이어 할당됨: " + player.name);
    }
}
