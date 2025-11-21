using TMPro;
using UnityEngine;

public class InteractTutorialText : MonoBehaviour, IInteractable
{
    public string GetPromptText()
    {
        return "[A] 텍스트 끄기";
    }

    public void Interact()
    {
        gameObject.SetActive(false);
    }
}
