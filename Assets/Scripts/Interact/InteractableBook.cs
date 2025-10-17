using UnityEngine;
using TMPro;

public class InteractableBook : MonoBehaviour, IInteractable
{
    public GameObject hintUI;           // 펼쳐진 책 UI    
    private bool hasRead = false;       // 책을 이미 읽었는지     
    private bool isHintOpen = false;    // 책 UI가 열려 있는지

    public void Interact()
    {
        // 책 펼쳐져 있으면 닫기
        if (isHintOpen)
        {
            hintUI.SetActive(false);
            isHintOpen = false;
        }
        // 책 처음 읽기
        else if (!hasRead)
        {
            hasRead = true;
            isHintOpen = true;
            hintUI.SetActive(true);
        }
    }

    public string GetPromptText()
    {
        if (isHintOpen) return "[E] 책 덮기";
        if (!hasRead) return "[E] 책 읽기";
        return ""; // 이미 읽었고, 닫힌 상태면 상호작용 없음
    }
    
    // InteractionManager에서 열림 상태 확인용
    public bool IsHintOpen()
    {
        return isHintOpen;
    }
}
