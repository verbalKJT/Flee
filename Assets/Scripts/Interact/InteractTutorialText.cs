using TMPro;
using UnityEngine;

public class InteractTutorialText : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject targetPanelToClose;
    public string GetPromptText()
    {
        return "설명을 닫으려면 A 버튼을 누르세요";
    }

    public void Interact()
    {
        Debug.Log("튜토리얼 패널을 닫습니다.");

        if (targetPanelToClose != null)
        {
            targetPanelToClose.SetActive(false); // 패널 비활성화
        }
        else
        {
            Debug.LogWarning("닫을 패널이 연결되지 않았습니다.");
        }
    }
}
