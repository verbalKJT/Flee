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
        return isOpen ? "[E] 문 닫기" : "[E] 문 열기";
    }

    //문 자동 닫힘 트리거
    private void OnTriggerEnter(Collider other)
    {
        //플레이어가 doorCloseTrigger에 닿으면 문닫힘 애니메이션 재생
        if (other.CompareTag("Player"))
        {
            isOpen = !isOpen;
            animator.SetBool("Open", isOpen);
        }
    }
}