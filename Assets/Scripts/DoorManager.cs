using UnityEngine;

public class DoorManager : MonoBehaviour, IInteractable
{
    public Animator animator;
    public string promptText = "[E] �� ����";

    private bool isOpen = false;

    public void Interact()
    {
        isOpen = !isOpen;
        animator.SetBool("Open", isOpen);
    }

    public string GetPromptText()
    {
        return isOpen ? "[E] �� �ݱ�" : "[E] �� ����";
    }
}