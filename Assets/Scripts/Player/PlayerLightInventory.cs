using UnityEngine;

public class PlayerLightInventory : MonoBehaviour
{
    [Header("플레이어 조명 오브젝트")]
    public GameObject candleLight;     // FlashHolder 밑 CandleLight
    public GameObject flashlightObject; // FlashHolder 밑 Flashlight

    [Header("손전등 획득 여부")]
    public bool hasFlashlight = false;

    void Start()
    {
        // 게임 시작: 양초불빛 켜기, 손전등 끄기
        SetModeCandle();
    }

    public void SetModeCandle()
    {
        hasFlashlight = false;

        if (candleLight != null)
            candleLight.SetActive(false);

        if (flashlightObject != null)
            flashlightObject.SetActive(false);
    }

    public void SetModeFlashlight()
    {
        hasFlashlight = true;

        if (candleLight != null)
            candleLight.SetActive(false);

        if (flashlightObject != null)
            flashlightObject.SetActive(true);
    }
}
