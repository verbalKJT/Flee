using UnityEngine;
using TMPro;

public class FindLighterInBox : MonoBehaviour, IInteractable
{
    public GameObject lighterIcon; // Inventory 확인용
    public static bool hasLighter = false; // 라이터를 이미 획득했는지   

    public void Interact()
    {
        // 라이터를 얻지 않은 경우  
        if (!hasLighter)
        {
            hasLighter = true;
            Debug.Log("라이터를 획득했다!");
            // 라이터 아이콘 UI가 있다면 표시   
            if (lighterIcon != null)
                lighterIcon.SetActive(true);
        }
    }

    public string GetPromptText()
    {
        return hasLighter ? "라이터를 획득했다!" : "[E] 상자 확인하기";
    }
}