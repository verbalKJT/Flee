using System.Collections;
using UnityEngine;

public class HeadTrackingLock : MonoBehaviour
{
    private OVRManager ovrManager;

    private void SetHeadTrackingActive(bool IsActive)
    {
        ovrManager = OVRManager.instance;
        if (ovrManager != null)
        {
            // 위치,회전 트래킹 비,활성화 
            ovrManager.usePositionTracking = IsActive;
            ovrManager.useRotationTracking = IsActive;
            if (IsActive)
            {
                // 트래킹 활성화 시 시점 중앙 재설정
                OVRManager.display.RecenterPose();
            }
        }
    }

    public void LockTracking(float time)
    {
        StartCoroutine(LockingCorutine(time));
    }
    public IEnumerator LockingCorutine(float time)
    {
        SetHeadTrackingActive(false);
        yield return new WaitForSeconds(time);
        SetHeadTrackingActive(true);
    }
}