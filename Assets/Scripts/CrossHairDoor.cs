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
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // ī�޶� �߽ɿ��� ���� ���
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, doorLayer))
        {
            DoorOpener door = hit.collider.GetComponentInParent<DoorOpener>(); // �� ������Ʈ ã��

            if (door != null)
            {
                interactionText.text = door.IsOpen() ? "[E] �� �ݱ�" : "[E] �� ����";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    door.ToggleDoor(); // �� ���ų� �ݱ�
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