using UnityEngine;

public class SwitchManager : MonoBehaviour, IInteractable
{
    [Header("애니메이션")]
    public Animator animator;
    private bool isOn = false;

    [Header("램프 오브젝트")]
    public GameObject lampOnObject;  // Lamp_ON 오브젝트
    public GameObject lampOffObject; // Lamp_OFF 오브젝트

    public void Interact()
    {
        isOn = !isOn;

        // 애니메이션 트리거
        if (animator != null)
            animator.SetTrigger(isOn ? "SwitchOn" : "SwitchOff");

        // 오브젝트 on/off 전환
        if (lampOnObject != null) lampOnObject.SetActive(isOn);
        if (lampOffObject != null) lampOffObject.SetActive(!isOn);
    }

    public string GetPromptText()
    {
        return isOn ? "[E] 스위치 끄기" : "[E] 스위치 켜기";
    }
}
