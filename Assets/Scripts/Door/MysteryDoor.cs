using UnityEngine;

public class MysteryDoor : MonoBehaviour
{
    [Header("닫힐 문 (DoorOpener 스크립트)")]
    public DoorOpener targetDoor;

    [Header("한 번만 발동할지 여부")]
    public bool onlyOnce = true;

    private bool hasTriggered = false;

    private void Reset()
    {
        // 이 오브젝트에 붙은 Collider를 자동으로 Trigger로 전환
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (onlyOnce && hasTriggered)
            return;

        hasTriggered = true;

        if (targetDoor != null)
        {
            // 방에 들어오면 문 강제로 닫고 잠금
            targetDoor.ForceCloseDoor();
            targetDoor.LockDoor();
            Debug.Log("MysteryDoor: 문 강제 닫힘 + 잠김");
        }
        else
        {
            Debug.LogWarning("MysteryDoor: targetDoor가 설정되지 않았습니다.");
        }
    }
}