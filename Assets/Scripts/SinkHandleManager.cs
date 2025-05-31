using UnityEngine;

public class SinkHandleManager : MonoBehaviour, IInteractable
{
    [Header("�ִϸ��̼�")]
    public Animator animator;
    public string Open = "Open"; 

    [Header("���� ������Ʈ")]
    public GameObject waterStream;
    public WaterManager waterManager;

    [Header("Ÿ�̹� ����")]
    public float delayBeforeRise = 3f;

    [Header("���� ������ (������Ʈ ���)")]
    public FlickerLampManager flickerLamp;


    private bool activated = false;

    public void Interact()
    {
        
        if (activated) return;

        activated = true;
        Debug.Log("������ ��ȣ�ۿ��");

        // �ִϸ��̼� bool �Ķ���� Open= true
        if (animator != null)
        {
            animator.SetBool(Open, true);
        }

        // ���ٱ� ȿ�� On
        if (waterStream != null)
        {
            waterStream.SetActive(true);
        }

        // 4. ���� �����̱� ����
        if (flickerLamp != null)
        {
            flickerLamp.StartFlicker();
        }


        // n�� �� �� �������� ����
        Invoke(nameof(StartWaterRise), delayBeforeRise);
    }

    void StartWaterRise()
    {
        if (waterManager != null)
        {
            waterManager.StartRising();
            Debug.Log("���� �������� ������");
        }
    }

    public string GetPromptText()
    {
        return "[E] �� Ʋ��";
    }
}
