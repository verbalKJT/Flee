using UnityEngine;

public class FlashStun : MonoBehaviour
{
    [Header("실제 빛 켜지는 Light 컴포넌트")]
    public Light flashlightLight;

    [Header("스턴 레이 범위")]
    public float stunRange = 15f;

    [Header("몇 초마다 한 번씩만 스턴 체크 할지")]
    public float checkInterval = 0.1f;

    [Header("맞출 레이어 (Enemy 레이어만 쓰면 좋음)")]
    public LayerMask enemyLayers = ~0;

    [Header("레이 방향에 손전등도 맞출지 여부")]
    public bool alignFlashToRay = false;   // 원하면 켜기

    private float nextCheckTime = 0f;
    private Camera cam;

    //손전등 토글 스크립트 참조
    private FlashlightToggle flashlightToggle;
    void Awake()
    {
        cam = Camera.main;   // 메인 카메라 자동 찾기 (Cinemachine이 움직이는 그 카메라)
        flashlightToggle = GetComponentInParent<FlashlightToggle>();
    }

    void Reset()
    {
        flashlightLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (Time.time < nextCheckTime) return;
        nextCheckTime = Time.time + checkInterval;

        if (flashlightLight == null || !flashlightLight.enabled) return;

        if (flashlightToggle != null && flashlightToggle.IsFlickering)
            return;

        // 혹시 씬 재로드 등으로 날아갔을 때 대비
        if (cam == null) cam = Camera.main;
        if (cam == null) return;   // 진짜 카메라 없으면 그냥 종료

        // 1. "화면에서 마우스 포인터 위치" 기준 레이 만들기
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos); // ← 여기서 상하/좌우 다 반영됨

        Vector3 origin = ray.origin;
        Vector3 dir = ray.direction;

        // 손전등도 이 레이 방향을 보게 하고 싶으면
        if (alignFlashToRay)
        {
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        // 디버그용 레이 (Game/Scene 뷰에서 Gizmos 켜야 보임)
        Debug.DrawRay(origin, dir * stunRange, Color.cyan);

        // 2. 그 레이가 적을 맞추면 스턴
        if (Physics.Raycast(origin, dir, out RaycastHit hit, stunRange, enemyLayers))
        {
            EnemyAI enemy = hit.collider.GetComponentInParent<EnemyAI>();
            if (enemy != null)
            {
                enemy.StunByFlashlight();
            }
        }
    }
}