using System;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightPickup : MonoBehaviour
{
    [Header("PC 상호작용 키")]
    public KeyCode interactKey = KeyCode.E;
    
    [SerializeField] private GameObject flashlight; // 왼쪽 컨트롤러에 손전등 오브젝트
    bool pickedUp = false;
    
    [SerializeField] private GameObject flashlightUI; // 손전등 게이지
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
            Destroy(gameObject);           // 
        }
    }
*/
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("LeftController"))
        {
            flashlight.SetActive(true); // 손에 달려있는 손전등
            flashlightUI.SetActive(true);
            pickedUp = true;
            Destroy(gameObject); 
            other.GetComponentInParent<PlayerLightInventory>().SetModeFlashlight(); // 여기서 hasFlashlight = true;
            other.GetComponentInParent<LeftHandGrabFlash>().StartAnim();
        }
    }

    public void SetFlashlight(GameObject flashlight, GameObject flashlightUI)
    {
        this.flashlight = flashlight;
        this.flashlightUI = flashlightUI;
    }
}
