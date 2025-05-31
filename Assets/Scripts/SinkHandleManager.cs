using UnityEngine;

public class SinkHandleManager : MonoBehaviour, IInteractable
{
    [Header("애니메이션")]
    public Animator animator;
    public string Open = "Open"; 

    [Header("연출 오브젝트")]
    public GameObject waterStream;
    public WaterManager waterManager;

    [Header("타이밍 설정")]
    public float delayBeforeRise = 3f;

    [Header("램프 깜빡임 (오브젝트 기반)")]
    public FlickerLampManager flickerLamp;


    private bool activated = false;

    public void Interact()
    {
        
        if (activated) return;

        activated = true;
        Debug.Log("손잡이 상호작용됨");

        // 애니메이션 bool 파라미터 Open= true
        if (animator != null)
        {
            animator.SetBool(Open, true);
        }

        // 물줄기 효과 On
        if (waterStream != null)
        {
            waterStream.SetActive(true);
        }

        // 4. 램프 깜빡이기 시작
        if (flickerLamp != null)
        {
            flickerLamp.StartFlicker();
        }


        // n초 후 물 차오르기 시작
        Invoke(nameof(StartWaterRise), delayBeforeRise);
    }

    void StartWaterRise()
    {
        if (waterManager != null)
        {
            waterManager.StartRising();
            Debug.Log("물이 차오르기 시작함");
        }
    }

    public string GetPromptText()
    {
        return "[E] 물 틀기";
    }
}
