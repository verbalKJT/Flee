using UnityEngine;

public class MirrorMonTouch : MonoBehaviour
{
    // VR 손의 Collider에 "VRHand" 태그를 붙여야 합니다.
    private const string HandTag = "RightController";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(HandTag))
        {
            Debug.Log("플레이어 손과 접촉! MirrorMon 제거.");

            // 일정 시간 후 사라지게 하려면 StartCoroutine(FadeOutAndDestroy()); 사용
            gameObject.SetActive(false); // MirrorMon 비활성화
        }
    }
}
