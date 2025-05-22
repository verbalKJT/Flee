using UnityEngine;

public class ToiletManager : MonoBehaviour, IInteractable
{
    [Header("�ִϸ��̼�")]
    public Animator animator;
    private bool isOpen = false;

    public void Interact()
    {
        isOpen = !isOpen;
        Debug.Log("�� ���� �õ�");
        animator.SetBool("Open", isOpen);
    }

    public string GetPromptText()
    {
        return isOpen ? "[E] Ŀ�� �ݱ�" : "[E] Ŀ�� ����";
    }
}
