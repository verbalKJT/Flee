using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DeadGameover : MonoBehaviour, IDeadMon
{
    [Header("Timeline & UI")] [SerializeField]
    private PlayableDirector timeline;

    private GameObject gameOverUI;

    [Header("Player Control")] private GameObject playerObject; // Player 오브젝트
    private MonoBehaviour playerControllerScript; // PlayerMovement

    private bool hasPlayed = false;

    [Header("Player Respwan 위치")] [SerializeField]
    private Transform playerRespwan;

    void Start()
    {
        if (timeline != null)
            timeline.stopped += OnTimelineFinished;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            hasPlayed = true;

            // 플레이어 조작 끄기
            if (playerControllerScript != null)
                playerControllerScript.enabled = false;

            // Timeline 실행
            if (timeline != null)
                timeline.Play();
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        if (gameOverUI == null) return;
        
        CanvasGroup canvasGroup = gameOverUI.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            StartCoroutine(FadeInUI(canvasGroup, 1.9f)); // 2초 동안 페이드 인
        }
        else
        {
            GameManager.instance.PlayerReSpawn(playerRespwan, playerObject);
            gameOverUI.SetActive(true);
            Destroy(gameObject); // fallback
        }
    }

    private IEnumerator FadeInUI(CanvasGroup canvasGroup, float duration)
    {
        Debug.Log(canvasGroup);
        float elapsed = 0f;
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null; // 한 프레임 기다리기 
        }

        canvasGroup.alpha = 1f;
        
        GameManager.instance.PlayerReSpawn(playerRespwan, playerObject); // 플레이어 리스폰
        playerControllerScript.enabled = true; // 리스폰 후 플레이어 이동 활성화
        hasPlayed = false; // 리스폰 후 다시 OnTriggerEnter가 실행될 수 있도록
    }

    // 동적할당 후 바인딩 메소드
    public void SetTrackBinding(GameObject mainCamera, GameObject moster)
    {
        if (timeline == null)
        {
            Debug.Log("Timeline is null");
            return;
        }

        // 트랙정보 가져오기
        TimelineAsset timelineAsset = timeline.playableAsset as TimelineAsset;
        if (timelineAsset == null)
        {
            Debug.Log("timeline asset is null");
        }

        // Track 정보 담을 변수
        TrackAsset cameraTrack = null;
        TrackAsset monsterTrack = null;

        // 타임라인의 목록 전부 가져와서 시네머신 트랙 찾기
        IEnumerable<TrackAsset> tracks = timelineAsset.GetOutputTracks();

        if (tracks != null)
        {
            foreach (TrackAsset track in tracks)
            {
                if (track.name.Contains("Cinemachine Track"))
                {
                    cameraTrack = track; // 트택 찾아서 저장
                }
                else if (track.name.Contains("MainMon") || track.name.Contains("Middle Mon"))
                {
                    monsterTrack = track;
                }
            }
        }

        // 저장한 트랙 바인딩
        if (cameraTrack != null && monsterTrack != null)
        {
            timeline.SetGenericBinding(cameraTrack, mainCamera.gameObject.GetComponent<CinemachineBrain>());
            timeline.SetGenericBinding(monsterTrack, moster.GetComponent<Animator>());
        }
    }


    public void SetPlayerWithUi(GameObject playerObject, MonoBehaviour playerControllerScript, GameObject gameOverUI)
    {
        this.playerObject = playerObject;
        this.playerControllerScript = playerControllerScript;
        this.gameOverUI = gameOverUI;
    }
}