using System;
using UnityEngine;

public class FlashlightPickup : MonoBehaviour
{
    [Header("��ȣ�ۿ� Ű")]
    public KeyCode interactKey = KeyCode.E;
    
    [SerializeField] private GameObject flashlight; // 왼쪽 컨트롤러에 손전등 오브젝트
    bool pickedUp = false;
/*
    void OnTriggerStay(Collider other)
    {
        if (pickedUp) return;
        if (!other.CompareTag("Player")) return;
        // E 키 또는 A 버튼으로 상호작용
        if (Input.GetKeyDown(interactKey) || OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
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
*/
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("LeftController"))
        {
            flashlight.SetActive(true);

            pickedUp = true;
            Destroy(gameObject); 
            
            other.GetComponentInParent<LeftHandGrabFlash>().StartAnim();
        }
    }

    public void SetFlashlight(GameObject flashLight)
    {
        this.flashlight = flashLight;
    }
}
