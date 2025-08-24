using UnityEngine;
using TMPro;

public class FindLighterInBox : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 2f;
    public LayerMask interactLayer;
    public TextMeshProUGUI interactionText;
    public GameObject lighterIcon; // Inventory 확인용

    public static bool hasLighter = false;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            if (hit.collider.CompareTag("Box") && !hasLighter)
            {
                interactionText.text = "[E] 상자 확인하기";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hasLighter = true;
                    interactionText.text = "야생의 상자에서 라이터를 획득했다!!";
                    if (lighterIcon != null)
                        lighterIcon.SetActive(true); // UI 인벤토리 등
                    Invoke("HideText", 3f);
                }
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void HideText()
    {
        interactionText.gameObject.SetActive(false);
    }
}