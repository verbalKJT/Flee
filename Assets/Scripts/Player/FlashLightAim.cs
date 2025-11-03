using UnityEngine;

public class FlashlightAim : MonoBehaviour
{
    [Header("플레이어 카메라(또는 바라볼 기준)")]
    public Transform cameraTransform;

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // 위치는 손(부모)이 결정 → 회전만 카메라 바라보는 방향으로 덮어씀
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;                       // 위/아래로는 고정하고 싶으면 이 줄 유지
        forward.Normalize();

        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}