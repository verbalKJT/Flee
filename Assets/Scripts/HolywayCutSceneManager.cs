using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class HolywayCutsceneManager : MonoBehaviour
{
    [Header("🎬 컷씬 설정")]
    public PlayableDirector timeline;          // 실행시킬 타임라인
    public bool playOnce = true;               // 한 번만 실행되게 할지
    public bool autoPlayOnTrigger = true;      // 트리거 진입 시 자동 실행 여부

    [Header("🚪 문 제어 설정")]
    public DoorOpener targetDoor;              // 닫을 문 오브젝트
    public float doorCloseDelay = 1.0f;        // 닫기까지 딜레이 시간

    private bool hasPlayed = false;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        AutoAssignReferences();
    }

    private void AutoAssignReferences()
    {
        // Player 자동 탐색
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();

            // Player의 PlayableDirector 자동 연결
            if (timeline == null)
            {
                var playerDirector = player.GetComponent<PlayableDirector>();
                if (playerDirector != null)
                {
                    timeline = playerDirector;
                    Debug.Log("🎬 [AutoAssign] Player의 PlayableDirector 자동 연결 완료");
                }
            }
        }
        else
        {
            Debug.LogWarning("⚠️ [AutoAssign] Player 태그를 가진 오브젝트를 찾지 못했습니다!");
        }
            // (EndDoor 태그 자동 탐색)
        if (targetDoor == null)
        {
            var endDoor = GameObject.FindGameObjectWithTag("EndDoor");
            if (endDoor != null)
            {
                targetDoor = endDoor.GetComponent<DoorOpener>();
                if (targetDoor != null)
                    Debug.Log($"[AutoAssign] EndDoor 자동 연결 완료: {targetDoor.name}");
                else
                    Debug.LogWarning("[AutoAssign] EndDoor 오브젝트에 DoorOpener 컴포넌트가 없습니다!");
            }
            else
            {
                Debug.LogWarning("[AutoAssign] EndDoor 태그를 가진 오브젝트를 찾지 못했습니다!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!autoPlayOnTrigger) return;
        if (!other.CompareTag("Player")) return;
        if (hasPlayed && playOnce) return;

        hasPlayed = true;
        Debug.Log("컷씬 트리거 진입 — 타임라인 실행 및 문 닫기 시작");

        if (targetDoor != null)
            StartCoroutine(CloseDoorAfterDelay());

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator CloseDoorAfterDelay()
    {
        yield return new WaitForSeconds(doorCloseDelay);

        if (targetDoor != null)
        {
            targetDoor.ForceCloseDoor();
            Debug.Log($"[AutoClose] {targetDoor.name} 문 닫힘 완료");
        }
        else
        {
            Debug.LogWarning("닫을 문이 설정되어 있지 않습니다!");
        }
    }

    private IEnumerator PlayCutscene()
    {
        if (timeline == null)
        {
            Debug.LogWarning("타임라인이 지정되지 않았습니다!");
            yield break;
        }

        // ✅ 컷씬 시작 시 플레이어 이동 비활성화
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            Debug.Log("🧍 플레이어 이동 잠금 (컷씬 중)");
        }

        // ✅ Animator를 Idle 상태로 전환
        var animator = playerMovement != null ? playerMovement.GetComponent<Animator>() : null;
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            Debug.Log("컷씬 시작 — Animator를 Idle 상태로 전환");
        }


        timeline.RebuildGraph();
        yield return new WaitForEndOfFrame();

        timeline.Play();
        Debug.Log("컷씬 타임라인 재생 시작");

      
        yield return new WaitForSeconds((float)timeline.duration + 0.1f);

        timeline.Stop();
        Debug.Log("컷씬 타임라인 정지 완료");

        // ✅ 컷씬 종료 후 플레이어 이동 복원
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
            Debug.Log("컷씬 종료  플레이어 이동 복원");
        }
    }
}
