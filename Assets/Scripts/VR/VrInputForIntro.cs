using UnityEngine;
using UnityEngine.UI;

public class VrInputForIntro : MonoBehaviour
{
    private LayerMask _layerMask;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // LayerMask는 비트마스크로 설정해야함.
        _layerMask = 1 << LayerMask.NameToLayer("UI");
    }

    // Update is called once per frame
    void Update()
    {
        // UI 탐색
        TrackingUI();
    }

    private void TrackingUI()
    {
        // 
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30f, _layerMask))
        {
            // UI Layerㅇ
            GameObject hitObj = hit.transform.gameObject;

            Button button = hitObj.GetComponent<Button>();
            if (button != null && ARAVRInput.GetDown(ARAVRInput.Button.One,
                    ARAVRInput.Controller.RTouch))
            {
                button.onClick.Invoke();
            }
        }
    }
}