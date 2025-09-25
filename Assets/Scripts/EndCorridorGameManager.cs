using UnityEngine;
// using UnityEngine.SceneManagement; // 마지막에 씬 전환 쓸 거면 주석 해제

public class EndCorridorGameManager : MonoBehaviour
{
    public static EndCorridorGameManager I;
    void Awake() => I = this;

    [Header("Prefabs & Refs")]
    public GameObject holywayPrefab;   // 복도 프리팹 (Holyway 컴포넌트 포함)
    public Transform spawnRoot;        // 복도 생성 기준 위치/회전 (GameManager 자신을 써도 OK)
    public Transform player;           // 플레이어 Transform

    [Header("Progress")]
    public int totalStages = 4;        // 총 스테이지 개수 (1..4)
    private int currentStage = 1;      // 현재(다음에 생성할) 스테이지 번호
    private holyway current;           // 현재 살아있는 복도
    private CharacterController cc;
    private bool inTransition = false; // 중복 트리거 방지

    void Start()
    {
        cc = player.GetComponent<CharacterController>();

        // 시작: 첫 복도 생성 (처음엔 텔포를 하지 않는 요구라면 다음 두 줄 중 Teleport를 제거)
        current = SpawnNewRoom(currentStage);
        // 처음엔 텔포하지 않는다 → 주석 유지
        // TeleportTo(current.startPoint.position);
    }

    public void OnEndReached(holyway ended)
    {
        if (inTransition || ended != current) return;
        inTransition = true;

        // 다음 스테이지로 진행할지 결정
        if (currentStage >= totalStages)
        {
            OnFinishedAll();
            inTransition = false;
            return;
        }

        // 다음 복도 생성
        currentStage++;
        var next = SpawnNewRoom(currentStage);

        // ▶ 요구사항: 끝에 닿을 때만, 새 복도의 시작 점으로 텔포
        TeleportTo(next.startPoint.position);

        // 이전 복도 정리
        current.OnBeforeDestroyed();
        Destroy(current.gameObject);

        current = next;
        inTransition = false;
    }

    holyway SpawnNewRoom(int stageIndex)
    {
        var go = Instantiate(holywayPrefab, spawnRoot.position, spawnRoot.rotation, spawnRoot);
        var hw = go.GetComponent<holyway>();
        hw.OnSpawned(stageIndex); // 스테이지 번호 전달 → 스테이지별 가구 배치
        return hw;
    }

    void TeleportTo(Vector3 pos)
    {
        if (cc != null) { cc.enabled = false; player.position = pos; cc.enabled = true; }
        else player.position = pos;
    }

    void OnFinishedAll()
    {
        Debug.Log("[CorridorGameManager] 모든 스테이지 통과 완료!");
        // 필요 시 씬 전환/보스전 등
        // SceneManager.LoadScene("NextSceneName");
    }
}
