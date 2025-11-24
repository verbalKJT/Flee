using UnityEngine;
using UnityEngine.UI;

public class FlashLightGaze : MonoBehaviour
{
    private PlayerLight playerLight;

    [Header("시간을 나타낼 UI Image")] [SerializeField]
    private Image flashGaze;

    // overheatTime, cooldownTime // 20, 3
    // onTimer, offTimer -> 켜져있는 시간 끄고 있는 시간 Time.deltaTime
    void Start()
    {
        playerLight = GetComponent<PlayerLight>();
    }


    void Update()
    {
        if (playerLight == null || flashGaze == null)
        {
            return;
        }

        // 비율 가져오기
        float heatRatio = playerLight.GetHeatRatio();
        
        // 이미지가 줄어들도록
        flashGaze.fillAmount = 1f - heatRatio;

        if (flashGaze.fillAmount <= 0.3f)
        {
            flashGaze.color = Color.red;
        }
        else
        {
            flashGaze.color = Color.white;
        }
    }
}