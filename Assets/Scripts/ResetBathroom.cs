using UnityEngine;

public class ResetBathroom : MonoBehaviour
{
    [Header("���� ��� ������Ʈ��")]
    public GameObject Door;
    public GameObject Wall_Trigger;
    public GameObject Lamp;
    public GameObject WaterStream;
    public WaterManager waterManager;
    public GameObject Water_Cam;

    [Header("�ʱ� �� ��ġ")]
    private Vector3 waterInitialPosition;

    void Start()
    {
        // ���� ���� �� �� ��ġ ����
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

        Debug.Log("�� �ʱ�ȭ �Ϸ�");
    }
}
