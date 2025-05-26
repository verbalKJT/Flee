using UnityEngine;

public class DoorManager : MonoBehaviour, IInteractable
{
    public Animator animator;
    public string promptText = "[E] 문 열기";

    private bool isOpen = false;

    public void Interact()
    {
        isOpen = !isOpen;
        animator.SetBool("Open", isOpen);
    }

    public string GetPromptText()
    {
        return isOpen ? "[E] 문 닫기" : "[E] 문 열기";
    }
}