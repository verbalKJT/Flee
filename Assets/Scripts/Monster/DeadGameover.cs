using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DeadGameover : MonoBehaviour, IDeadMon
{
    [Header("Timeline & UI")] [SerializeField]
    private PlayableDirector timeline;

    [Header("Player Control")] private GameObject playerObject; // Player 오브젝트
    private MonoBehaviour playerControllerScript; // PlayerMovement

    private bool hasPlayed = false;

    [Header("Player Respwan 위치")] [SerializeField]
    private Transform playerRespwan;

    void Start()
    {
        if (timeline != null)
            timeline.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector obj)
    {
        StartCoroutine(CallReSpawn());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            hasPlayed = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = false; // Collider 비활성화 추가 실행 방지
            gameObject.GetComponent<NavMeshAgent>().isStopped = true; //몬스터들 이동 멈추기 

            // 플레이어 조작 끄기
            if (playerControllerScript != null)
                playerControllerScript.enabled = false;

            // Timeline 실행
            if (timeline != null)
            {
                timeline.Play();
            }
        }
    }

    // 플레이어가 리스폰 되어서 몬스터와 떨어졌을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hasPlayed)
        {
            // 플레이어가 콜라이더 밖으로 완전히 나갔을 때만 리셋
            hasPlayed = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = true; // 콜라이더 활성화
            gameObject.GetComponent<NavMeshAgent>().isStopped = false; // 이동 재개
        }
    }

    private IEnumerator CallReSpawn()
    {
        Debug.Log("CallReSpawn 코루틴 시작");
        yield return new WaitForSeconds(0.1f);

        // null 체크
        if (GameManager.instance == null) Debug.LogError("GameManager.instance is null!");
        if (playerRespwan == null) Debug.LogError("playerRespwan is null!");
        if (playerObject == null) Debug.LogError("playerObject is null!");

        if (playerRespwan != null && playerObject != null && GameManager.instance != null)
        {
            GameManager.instance.PlayerReSpawn(playerRespwan, playerObject);
            playerControllerScript.enabled = true;
            Debug.Log("플레이어 리스폰 성공");
        }
    }

    public void SetPlayerWithUi(GameObject playerObject, MonoBehaviour playerControllerScript)
    {
        this.playerObject = playerObject;
        this.playerControllerScript = playerControllerScript;
    }

    public void SetTrackBinding(GameObject mainCamera, GameObject overPanel)
    {
        // 트랙정보 가져오기
        TimelineAsset timelineAsset = timeline.playableAsset as TimelineAsset;
        if (timelineAsset == null)
        {
            Debug.LogWarning("timeline asset is null");
        }

        // Track 정보 담을 변수
        TrackAsset cameraTrack = null;

        // 타임라인의 목록 전부 가져와서 시네머신 트랙 찾기
        IEnumerable<TrackAsset> tracks = timelineAsset.GetOutputTracks();
        
        if (tracks != null)
        {
            foreach (TrackAsset track in tracks)
            {
                // Cinemachine Track을 찾아서 Main Camera의 CinemachineBrain에 바인딩
                if (track is CinemachineTrack)
                {
                    CinemachineBrain brain = mainCamera.GetComponent<CinemachineBrain>();
                    if (brain != null)
                    {
                        timeline.SetGenericBinding(track, brain);
                        Debug.Log(gameObject.name + "Cinemachine Track bound to CinemachineBrain.");
                    }
                }
                else if (track is AnimationTrack)
                {
                    AnimationTrack animator = mainCamera.GetComponent<AnimationTrack>();
                    timeline.SetGenericBinding(track,animator);
                }
                else if (track is ActivationTrack || track.name.Contains("Panel"))
                {
                    timeline.SetGenericBinding(track, overPanel);
                }
            }
        }
    }
}