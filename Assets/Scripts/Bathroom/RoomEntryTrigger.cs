using UnityEngine;

public class RoomEntryTrigger : MonoBehaviour
{
    //방에 들어왔을 때 문이 벽으로 변하는 기믹 수행
    [Header("전환 대상")]
    public GameObject doorObject;  // 기존 문 오브젝트
    public GameObject wallObject;  // 문 대신 보여줄 벽

    public bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        //트리거에 닿지 않았다면 동작 x
        if (triggered) return;

        //플레이어가 닿는다면
        if (other.CompareTag("Player"))
        {
            triggered = true;

            //문오브젝트 비활성화
            if (doorObject != null)
                doorObject.SetActive(false);

            //벽 오브젝트 활성화
            if (wallObject != null)
                wallObject.SetActive(true);

            Debug.Log("방 입장 - 문 숨기고 벽 활성화됨");
        }
    }
}
