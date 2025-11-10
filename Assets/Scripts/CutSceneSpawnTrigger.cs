using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneSpawnTrigger : MonoBehaviour
{
    [Header("Timeline 설정")]
    public PlayableDirector director; // EndCorridorCutScene 연결 

    [Header("몬스터 설정")]
    public GameObject monsterPrefab;  // 소환할 몬스터 프리팹
    public Transform spawnPoint;      // 소환 위치 (빈 오브젝트)

    private bool hasSpawned = false;

    // 컷씬에서 생성된 몬스터들을 저장하는 전역 리스트
    public static List<GameObject> spawnedMonsters = new();

    void Awake()
    {
        AutoAssignDirector();
    }

    void Start()
    {
        if (director != null)
        {
            // 컷씬이 끝날 때 호출되는 이벤트 등록
            director.stopped += OnCutsceneEnd;
        }
        else
        {
            Debug.LogWarning("⚠️ [CutSceneSpawnTrigger] PlayableDirector가 설정되지 않았습니다.");
        }
    }

    private void AutoAssignDirector()
    {
        // Player 오브젝트 자동 탐색
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("⚠️ [CutSceneSpawnTrigger] Player 태그 오브젝트를 찾을 수 없습니다!");
            return;
        }

        // Player의 PlayableDirector 자동 연결
        if (director == null)
        {
            var playerDirector = player.GetComponent<PlayableDirector>();
            if (playerDirector != null)
            {
                director = playerDirector;
                Debug.Log($"Player의 PlayableDirector 자동 연결 완료 → {playerDirector.name}");
            }
            else
            {
                Debug.LogWarning("Player 오브젝트에 PlayableDirector가 없습니다!");
            }
        }
    }

    private void OnCutsceneEnd(PlayableDirector obj)
    {
        if (!hasSpawned && monsterPrefab != null && spawnPoint != null)
        {
            hasSpawned = true;
            var monster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
            spawnedMonsters.Add(monster); // ✅ 리스트에 추가

        }
    }

    private void OnDestroy()
    {
        if (director != null)
            director.stopped -= OnCutsceneEnd;
    }
}
