using UnityEngine;

public class FlashlightPickup : MonoBehaviour
{
    [Header("��ȣ�ۿ� Ű")]
    public KeyCode interactKey = KeyCode.E;

    bool pickedUp = false;

    void OnTriggerStay(Collider other)
    {
        if (pickedUp) return;
        if (!other.CompareTag("Player")) return;

        // E 키 또는 A 버튼으로 상호작용
        if (Input.GetKeyDown(interactKey) || OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            var inv = other.GetComponent<PlayerLightInventory>();
            if (inv != null)
            {
                inv.SetModeFlashlight();   // 손전등을 켜는 로직 
            }
            
            pickedUp = true;
            Destroy(gameObject);           // �ٴڿ� �ִ� ������ ������ ����
        }
    }
}
