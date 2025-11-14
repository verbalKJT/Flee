using UnityEngine;

public class LeftHandLighter : MonoBehaviour
{
    [Header("왼손에서 사용할 도구들")]
    public GameObject candleObject;       // 비활성화한 상태로 플레이어 손 아래 존재
    public GameObject flashlightObject;   // 비활성화한 상태로 플레이어 손 아래 존재

    [Header("현재 활성화된 도구")]
    public GameObject currentTool;
    
    // 촛불 활성화
    public void UseCandle()
    {
        SetActiveTool(candleObject);
        // ➤ 촛불도 왼손 컨트롤러에 붙이기
        if (candleObject != null && ARAVRInput.LHand != null)
        {
            candleObject.transform.SetParent(ARAVRInput.LHand);
            candleObject.transform.localPosition = Vector3.zero;
            candleObject.transform.localRotation = Quaternion.identity;
        }
    }
    // 손전등 활성화
    public void UseFlashlight()
    {
        SetActiveTool(flashlightObject);
        // ➤ 손전등이 왼손 컨트롤러 위치에 정확히 붙도록 조정
        if (flashlightObject != null && ARAVRInput.LHand != null)
        {
            flashlightObject.transform.SetParent(ARAVRInput.LHand);  // 왼손 컨트롤러에 붙이기
            flashlightObject.transform.localPosition = Vector3.zero;
            flashlightObject.transform.localRotation = Quaternion.identity;
        }
    }
    // 모든 왼손 도구 비활성화
    public void DisableAll()
    {
        if (candleObject) candleObject.SetActive(false);
        if (flashlightObject) flashlightObject.SetActive(false);
        currentTool = null;
    }
    // 지정된 도구만 활성화
    private void SetActiveTool(GameObject tool)
    {
        DisableAll(); // 모두 끄고
        tool.SetActive(true); // 원하는 것만 켬
        currentTool = tool;
    }
}