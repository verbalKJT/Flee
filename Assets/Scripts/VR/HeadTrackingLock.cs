using System.Collections;
using UnityEngine;

public class HeadTrackingLock : MonoBehaviour
{
    private OVRManager ovrManager;
    private bool isLockedTracking;

    void Start()
    {
        ovrManager = FindObjectOfType<OVRManager>();
    }
    private void SetHeadTrackingActive(bool IsActive)
    {
        if (ovrManager != null)
        {
            // 위치,회전 트래킹 비,활성화 
            ovrManager.usePositionTracking = IsActive;
            ovrManager.useRotationTracking = IsActive;
            
            // 트래킹 상태 설정
            isLockedTracking = !IsActive;

            if (IsActive)
            {
                // 트래킹 활성화 시 시점 중앙 재설정
                OVRManager.display.RecenterPose();
                Debug.Log("Recentered");
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
        isLockedTracking = true;
        yield return new WaitForSeconds(time);
        SetHeadTrackingActive(true);
        isLockedTracking = false;
    }

    void LateUpdate()
    {
        if (ovrManager != null && isLockedTracking)
        {
            ovrManager.usePositionTracking = false;
            ovrManager.useRotationTracking = false;
        }
    }
}