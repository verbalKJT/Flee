using UnityEngine;

public class EndlessHallway : MonoBehaviour
{
    public Transform player;
    public Transform teleportPoint;
    public GameObject[] variations;
    public Light hallwayLight;

    private int current = 0;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 만약 이미 마지막 복도일때 탈출 
        if (current == variations.Length - 1)
        {
            return;
        }
        // 텔포 위치 어디서 뜨지
        Debug.Log($"[Teleport] From {player.position} to {teleportPoint.position}");

        // CharacterController 가져오기
        var cc = player.GetComponent<CharacterController>();
        if (cc != null) // 잠깐 껐다가 텔포시키고 그 다음에 플레이어가 움직일수 있도록 제어
        {
            cc.enabled = false;                          
            player.position = teleportPoint.position;    
            cc.enabled = true;                           
        }
        else
        {
            // 위치 교체 텔포
            player.position = teleportPoint.position;
        }

        // 복도 전환 하기 배열로 만들어 놓은것들
        current = (current + 1) % variations.Length;
        for (int i = 0; i < variations.Length; i++)
            variations[i].SetActive(i == current);

    }
}
