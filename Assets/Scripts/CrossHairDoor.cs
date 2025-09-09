using TMPro;
using UnityEngine;

using UnityEngine;
using TMPro;

public class CrossHairDoor : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask doorLayer;
    public TextMeshProUGUI interactionText;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 카메라 중심에서 광선 쏘기
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, doorLayer))
        {
            DoorOpener door = hit.collider.GetComponentInParent<DoorOpener>(); // 문 컴포넌트 찾기

            if (door != null)
            {
                interactionText.text = door.IsOpen() ? "[E] 문 닫기" : "[E] 문 열기";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    door.ToggleDoor(); // 문 열거나 닫기
                }
            }
            else
            {
                interactionText.gameObject.SetActive(false);
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }
}