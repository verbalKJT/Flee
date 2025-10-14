using UnityEngine;

public class ResetBathroom : MonoBehaviour
{
    [Header("리셋 대상 오브젝트들")]
    public GameObject Door;
    public GameObject Wall_Trigger;
    public GameObject Lamp;
    public GameObject WaterStream;
    public WaterManager waterManager;
    public GameObject Water_Cam;

    [Header("초기 물 위치")]
    private Vector3 waterInitialPosition;

    void Start()
    {
        // 게임 시작 시 물 위치 저장
        if (waterManager != null)
        {
            waterInitialPosition = waterManager.transform.position;
        }
    }

    public void ResetRoom()
    {
        //문 오브젝트 활성화
        if (Door != null) Door.SetActive(true);
        if (Wall_Trigger != null) Wall_Trigger.SetActive(false);

        //불 깜빡임 정지
        if (Lamp != null)
        {
            var flicker = Lamp.GetComponent<FlickerLampManager>();
            if (flicker != null) flicker.StopFlicker();
        }

        //세면대 물줄기 비활성화
        if (WaterStream != null) WaterStream.SetActive(false);

        //떠오르는 물 멈추고 시작위치로 위치 변경
        if (waterManager != null)
        {
            waterManager.StopRising();
            waterManager.transform.position = waterInitialPosition; 
        }

        //물에 잠기는 효과 비활성화
        if (Water_Cam != null)
        {
            var cg = Water_Cam.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 0f;
            Water_Cam.SetActive(false);
        }

        Debug.Log("방 초기화 완료");
    }
}
