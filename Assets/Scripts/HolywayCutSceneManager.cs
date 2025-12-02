using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HolywayCutsceneManager : MonoBehaviour
{
    [Header("🎬 컷씬 설정")]
    public PlayableDirector timeline;
    public bool playOnce = true;
    public bool autoPlayOnTrigger = true;

    [Header("🚪 문 제어 설정")]
    public DoorOpener targetDoor;
    public float doorCloseDelay = 1.0f;

    private FootstepSound[] crawlFootsteps;
    private AudioSource[] crawlAudioSources;

    private bool hasPlayed = false;
    private bool isCutscenePlaying = false;

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
                var pd = player.GetComponent<PlayableDirector>();
                if (pd != null)
                {
                    timeline = pd;
                    Debug.Log("🎬 [AutoAssign] Player의 PlayableDirector 자동 연결 완료");
                }
            }
        }

        // EndDoor 자동 연결
        if (targetDoor == null)
        {
            var endDoorObj = GameObject.FindGameObjectWithTag("EndDoor");
            if (endDoorObj != null)
                targetDoor = endDoorObj.GetComponent<DoorOpener>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!autoPlayOnTrigger) return;
        if (!other.CompareTag("Player")) return;
        if (hasPlayed && playOnce) return;

        hasPlayed = true;

        if (targetDoor != null)
            StartCoroutine(CloseDoorAfterDelay());

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator CloseDoorAfterDelay()
    {
        yield return new WaitForSeconds(doorCloseDelay);

        if (targetDoor != null)
            targetDoor.ForceCloseDoor();
    }

    private IEnumerator PlayCutscene()
    {
        if (timeline == null)
        {
            Debug.LogWarning("타임라인이 지정되지 않았습니다!");
            yield break;
        }

        isCutscenePlaying = true;

        // 플레이어 이동 잠금
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Animator를 Idle로 강제
        var animator = playerMovement != null ? playerMovement.GetComponent<Animator>() : null;
        if (animator != null)
            animator.SetFloat("Speed", 0f);

        // CrawlMon 관련 사운드 구성요소 찾기
        TryFindCrawlFootsteps();
        TryFindCrawlAudioSources();

        // 소리 차단
        MuteCrawlFootsteps();
        MuteCrawlAudio();

        timeline.RebuildGraph();
        yield return new WaitForEndOfFrame();
        timeline.Play();

        // 타임라인 정지까지 대기
        yield return new WaitForSeconds((float)timeline.duration + 0.1f);

        timeline.Stop();

        // 컷씬 종료 → 소리 재활성화
        UnmuteCrawlFootsteps();
        UnmuteCrawlAudio();

        // 이동 복원
        if (playerMovement != null)
            playerMovement.enabled = true;

        isCutscenePlaying = false;
    }

    private void Update()
    {
        // 타임라인 중에는 활성화되는 CrawlMon 오브젝트를 계속 점검하여 즉시 소리 차단
        if (isCutscenePlaying)
        {
            TryFindCrawlFootsteps();
            TryFindCrawlAudioSources();

            MuteCrawlFootsteps();
            MuteCrawlAudio();
        }
    }

    // FootStepSound 및 AudioSource 컴포넌트 탐색
    private void TryFindCrawlFootsteps()
    {
        if (crawlFootsteps != null && crawlFootsteps.Length > 0)
            return;

        var crawls = GameObject.FindGameObjectsWithTag("CrawlMon");
        List<FootstepSound> list = new List<FootstepSound>();

        foreach (var c in crawls)
            list.AddRange(c.GetComponentsInChildren<FootstepSound>(true));

        if (list.Count > 0)
            crawlFootsteps = list.ToArray();
    }

    private void TryFindCrawlAudioSources()
    {
        if (crawlAudioSources != null && crawlAudioSources.Length > 0)
            return;

        var crawls = GameObject.FindGameObjectsWithTag("CrawlMon");
        List<AudioSource> list = new List<AudioSource>();

        foreach (var c in crawls)
            list.AddRange(c.GetComponentsInChildren<AudioSource>(true));

        if (list.Count > 0)
            crawlAudioSources = list.ToArray();
    }

    // 소리 일절 차단
    private void MuteCrawlFootsteps()
    {
        if (crawlFootsteps == null) return;

        foreach (var f in crawlFootsteps)
            if (f != null)
                f.enabled = false;
    }

    private void MuteCrawlAudio()
    {
        if (crawlAudioSources == null) return;

        foreach (var a in crawlAudioSources)
            if (a != null)
                a.enabled = false;
    }

    // 소리 재생
    private void UnmuteCrawlFootsteps()
    {
        if (crawlFootsteps == null) return;

        foreach (var f in crawlFootsteps)
            if (f != null)
                f.enabled = true;
    }

    private void UnmuteCrawlAudio()
    {
        if (crawlAudioSources == null) return;

        foreach (var a in crawlAudioSources)
            if (a != null)
                a.enabled = true;
    }
}
