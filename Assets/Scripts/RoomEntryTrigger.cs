using UnityEngine;

public class RoomEntryTrigger : MonoBehaviour
{
    [Header("전환 대상")]
    public GameObject doorObject;  // 기존 문 오브젝트
    public GameObject wallObject;  // 문 대신 보여줄 벽

    public bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (doorObject != null)
                doorObject.SetActive(false);

            if (wallObject != null)
                wallObject.SetActive(true);

            Debug.Log("방 입장 - 문 숨기고 벽 활성화됨");
        }
    }
}
