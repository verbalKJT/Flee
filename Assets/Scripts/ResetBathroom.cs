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

        if (Door != null) Door.SetActive(true);
        if (Wall_Trigger != null) Wall_Trigger.SetActive(false);

        if (Lamp != null)
        {
            var flicker = Lamp.GetComponent<FlickerLampManager>();
            if (flicker != null) flicker.StopFlicker();
        }

        if (WaterStream != null) WaterStream.SetActive(false);

        if (waterManager != null)
        {
            waterManager.StopRising();
            waterManager.transform.position = waterInitialPosition; 
        }

        if (Water_Cam != null)
        {
            var cg = Water_Cam.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 0f;
            Water_Cam.SetActive(false);
        }

        Debug.Log("방 초기화 완료");
    }
}
