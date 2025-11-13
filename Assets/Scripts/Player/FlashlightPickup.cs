using UnityEngine;

public class FlashlightPickup : MonoBehaviour
{
    [Header("상호작용 키")]
    public KeyCode interactKey = KeyCode.E;

    bool pickedUp = false;

    void OnTriggerStay(Collider other)
    {
        if (pickedUp) return;
        if (!other.CompareTag("Player")) return;

        // 여기서 E 키 눌렀을 때만 줍기
        if (Input.GetKeyDown(interactKey))
        {
            var inv = other.GetComponent<PlayerLightInventory>();
            if (inv != null)
            {
                inv.SetModeFlashlight();   // 양초 불 끄고 손전등 모드로 전환
            }

            pickedUp = true;
            Destroy(gameObject);           // 바닥에 있는 손전등 아이템 제거
        }
    }
}
