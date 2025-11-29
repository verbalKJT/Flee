using UnityEngine;

public class FlashStun : MonoBehaviour
{
    [Header("실제 빛이 나가는 손전등 Light")]
    public Light flashlightLight;

    [Header("레이 시작 위치(손전등 머리 부분)")]
    public Transform rayOrigin;  // 안 넣으면 flashlightLight 위치 사용

    [Header("레이 로컬 방향 (모델 축에 맞게 조정)")]
    public Vector3 rayLocalDirection = Vector3.forward; // 예: (0,1,0) 이면 로컬 Y+ 방향

    [Header("스턴 레이 범위 / 굵기")]
    public float stunRange = 15f;
    public float stunRadius = 0.3f;   // 판정 반경 (값 키우면 더 널널해짐)

    [Header("몇 초마다 한 번씩만 스턴 체크 할지")]
    public float checkInterval = 0.1f;

    [Header("맞출 레이어 (Enemy 레이어만 쓰면 좋음)")]
    public LayerMask enemyLayers = ~0;

    [Header("디버그 레이 표시 여부")]
    public bool debugRay = true;

    private float nextCheckTime = 0f;

    // 손전등 토글 스크립트 참조 (깜빡임/ON/OFF 상태 확인용)
    private PlayerLight playerLight;

    void Awake()
    {
        playerLight = GetComponentInParent<PlayerLight>();

        // rayOrigin 안 넣어줬으면 기본값으로 세팅
        if (rayOrigin == null)
        {
            if (flashlightLight != null)
                rayOrigin = flashlightLight.transform;
            else
                rayOrigin = transform;
        }
    }

    void Reset()
    {
        // 에디터에서 컴포넌트 추가할 때 자동으로 Light 찾아주기
        flashlightLight = GetComponentInChildren<Light>();

        if (flashlightLight != null)
            rayOrigin = flashlightLight.transform;
        else
            rayOrigin = transform;
    }

    void Update()
    {
        // 체크 간격
        if (Time.time < nextCheckTime) return;
        nextCheckTime = Time.time + checkInterval;

        // 손전등 꺼져 있으면 스턴 없음
        if (flashlightLight == null || !flashlightLight.enabled)
            return;

        // 깜빡이는 상태(기절 불가 상태)면 패스
        if (playerLight != null && playerLight.IsFlickering)
            return;

        if (rayOrigin == null)
            rayOrigin = transform;

        // === 1. 레이 시작점 / 방향 ===
        Vector3 origin = rayOrigin.position;   // 손전등 머리 위치
        // 로컬 방향벡터(rayLocalDirection)를 월드 방향으로 변환해서 사용
        Vector3 dir = rayOrigin.TransformDirection(rayLocalDirection.normalized);

        // 디버그용 레이 (중심선)
        if (debugRay)
        {
            Debug.DrawRay(origin, dir * stunRange, Color.cyan);
        }

        // === 2. 굵은 레이(SphereCast)로 몬스터 스턴 판정 ===
        if (Physics.SphereCast(origin, stunRadius, dir,
                               out RaycastHit hit,
                               stunRange, enemyLayers))
        {
            // 맞은 콜라이더에서 EnemyAI 찾기 (부모까지 포함)
            EnemyAI enemy = hit.collider.GetComponentInParent<EnemyAI>();
            if (enemy != null)
            {
                enemy.StunByFlashlight();
            }
        }
    }

    // 씬뷰에서 선택했을 때 레이 두께까지 보이게 하는 기즈모
    void OnDrawGizmosSelected()
    {
        if (!debugRay) return;
        if (rayOrigin == null)
        {
            if (flashlightLight != null)
                rayOrigin = flashlightLight.transform;
            else
                rayOrigin = transform;
        }

        Gizmos.color = Color.cyan;

        Vector3 origin = rayOrigin.position;
        Vector3 dir = rayOrigin.TransformDirection(rayLocalDirection.normalized);

        // 중심선
        Gizmos.DrawLine(origin, origin + dir * stunRange);

        // 시작점과 끝점에 반지름 표시
        Gizmos.DrawWireSphere(origin, stunRadius);
        Gizmos.DrawWireSphere(origin + dir * stunRange, stunRadius);
    }
}
