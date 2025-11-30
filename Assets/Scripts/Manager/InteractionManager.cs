using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    public Camera playerCamera;                        // 플레이어 시점 카메라
    public float interactDistance = 2f;                // 상호작용 거리
    public LayerMask interactLayer;                    // 상호작용 가능한 레이어
    public TextMeshProUGUI interactionText;            // UI 텍스트 오브젝트
    
    public Transform rightHandRayOrigin;  // RightHandAnchor 연결


    private IInteractable currentInteractable;         // 현재 보고 있는 상호작용 대상
    private IInteractable lastOpenedUIObject;          // UI가 열린 상태의 오브젝트 (책 등)
    void Update()
    {
        // 1. 입력 감지 (키보드 'E' 또는 VR 컨트롤러 'A' 버튼)
        // OVRInput.Button.One == 오른쪽 컨트롤러의 'A' 버튼입니다.
        bool isInteracting = Input.GetKeyDown(KeyCode.E) || 
                             ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch);
        //  UI가 열려 있으면 다른 입력 무시하고 E로 닫기
        if (lastOpenedUIObject != null && isInteracting)
        {
            lastOpenedUIObject.Interact();

            // 책이 닫혔으면 null   
            if (lastOpenedUIObject is InteractableBook book && !book.IsHintOpen())
            {
                lastOpenedUIObject = null;
            }

            return;
        }

        // 카메라 앞으로 Raycast 쏴서 상호작용 대상 확인
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward); //PC
        //Ray ray = new Ray(rightHandRayOrigin.position, rightHandRayOrigin.forward); // VR
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            // Ray에 맞은 오브젝트에서 IInteractable 찾기
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;

                // UI 텍스트 표시
                interactionText.text = currentInteractable.GetPromptText();
                interactionText.gameObject.SetActive(true);

                // 2. 상호작용 (E키 또는 VR 버튼)
                if (isInteracting)
                {
                    currentInteractable.Interact();

                    // 만약 책처럼 UI가 열린 상태면 기억해둠
                    if (interactable is InteractableBook book && book.IsHintOpen())
                    {
                        lastOpenedUIObject = book;
                    }
                }

                return;
            }
        }

        // 상호작용할 대상이 없으면 UI 숨기기
        currentInteractable = null;
        interactionText.gameObject.SetActive(false);
    }
}
