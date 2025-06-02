using UnityEngine;

public class DoorManager : MonoBehaviour, IInteractable
{
    public Animator animator;
    
    public bool isOpen = false;

    public void Interact()
    {
        isOpen = !isOpen;
        animator.SetBool("Open", isOpen);
    }

    public string GetPromptText()
    {
        return isOpen ? "[E] �� �ݱ�" : "[E] �� ����";
    }

    //�� �ڵ� ���� Ʈ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = !isOpen;
            animator.SetBool("Open", isOpen);
        }
    }
}