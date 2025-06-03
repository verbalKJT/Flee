using UnityEngine;

public class ToiletManager : MonoBehaviour, IInteractable
{
    [Header("애니메이션")]
    public Animator animator;
    private bool isOpen = false;

    public void Interact()
    {
        isOpen = !isOpen;
        Debug.Log("문 열림 시도");
        animator.SetBool("Open", isOpen);
    }

    public string GetPromptText()
    {
        return isOpen ? "[E] 커버 닫기" : "[E] 커버 열기";
    }
}
