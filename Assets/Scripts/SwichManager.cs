using UnityEngine;

public class SwitchManager : MonoBehaviour, IInteractable
{
    [Header("�ִϸ��̼�")]
    public Animator animator;
    private bool isOn = false;

    [Header("���� ������Ʈ")]
    public GameObject lampOnObject;  // Lamp_ON ������Ʈ
    public GameObject lampOffObject; // Lamp_OFF ������Ʈ

    public void Interact()
    {
        isOn = !isOn;

        // �ִϸ��̼� Ʈ����
        if (animator != null)
            animator.SetTrigger(isOn ? "SwitchOn" : "SwitchOff");

        // ������Ʈ on/off ��ȯ
        if (lampOnObject != null) lampOnObject.SetActive(isOn);
        if (lampOffObject != null) lampOffObject.SetActive(!isOn);
    }

    public string GetPromptText()
    {
        return isOn ? "[E] ����ġ ����" : "[E] ����ġ �ѱ�";
    }
}
