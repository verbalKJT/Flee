using UnityEngine;
using TMPro;

public class InteractableBook : MonoBehaviour
{
     public Camera playerCamera;
    public float interactDistance = 2f;
    public LayerMask interactLayer;
    public TextMeshProUGUI interactionText;
    public GameObject hintUI; // 펼쳐진 책 이미지   

    private bool hasRead = false;
    private bool isHintOpen = false;

    void Update()
    {
        // 가장 먼저 열려 있으면 닫는 처리
        if (isHintOpen && Input.GetKeyDown(KeyCode.E))
        {
            hintUI.SetActive(false);
            isHintOpen = false;
            return; // 아래 상호작용 로직은 실행 안 함
        }

        // Ray를 쏴서 책에 닿았는지 체크
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            if (hit.collider.CompareTag("Book") && !hasRead)
            {
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hasRead = true;
                    isHintOpen = true;
                    interactionText.gameObject.SetActive(false);
                    hintUI.SetActive(true);
                }
            }
            else
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
}
