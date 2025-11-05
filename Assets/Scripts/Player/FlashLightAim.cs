using UnityEngine;

public class FlashlightAim : MonoBehaviour
{
    public Transform playerOrigin;

    public float maxDistance = 50f;
    public float rotateSpeed = 20f;
    public LayerMask hitLayers = ~0;

    [Header("좌/우 오프셋 (음수 = 왼쪽, 양수 = 오른쪽)")]
    public float horizontalOffsetDegrees = -5f;   // 살짝 왼쪽으로 -5도 정도

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        if (playerOrigin == null)
            playerOrigin = transform.root;
    }

    void LateUpdate()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null || playerOrigin == null) return;

        // 화면에서 마우스 포인터로 레이 쏘는 부분은 그대로 유지
        Vector3 mousePos = Input.mousePosition;
        Ray screenRay = cam.ScreenPointToRay(mousePos);

        Vector3 targetPoint;
        if (Physics.Raycast(screenRay, out RaycastHit hit, maxDistance, hitLayers))
            targetPoint = hit.point;
        else
            targetPoint = screenRay.origin + screenRay.direction * maxDistance;

        // 플레이어 기준 방향
        Vector3 dir = (targetPoint - playerOrigin.position);
        if (dir.sqrMagnitude < 0.0001f) return;
        dir.Normalize();

        // 기본 회전
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);

        // 👉 여기서 수평으로 살짝 틀어준다 (Vector3.up 기준 회전)
        Quaternion offsetRot = Quaternion.AngleAxis(horizontalOffsetDegrees, Vector3.up);
        targetRot = offsetRot * targetRot;

        // 부드럽게 회전
        if (rotateSpeed <= 0f)
        {
            transform.rotation = targetRot;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }
    }
}