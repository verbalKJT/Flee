using UnityEngine;

public class GrabItem : MonoBehaviour, IInteractable
{
    public string GetPromptText()
    {
        return "잡기";
    }

    public void Interact()
    {
        // 아무 동작도 하지 않음 - 실제 잡기는 RightHandGrabber가 처리
        Debug.Log("GrabItem.Interact() 호출됨");
    }
}