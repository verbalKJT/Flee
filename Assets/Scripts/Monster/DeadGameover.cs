using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

public class DeadGameover : MonoBehaviour, IDeadMon
{
    [Header("Timeline & UI")] [SerializeField]
    private PlayableDirector timeline;

    [Header("Player Control")] private GameObject playerObject; // Player 오브젝트
    private MonoBehaviour playerControllerScript; // PlayerMovement

    public static bool hasPlayed = true;

    [Header("Player Respwan 위치")] [SerializeField]
    private Transform playerRespwan;

    // 몬스터들 타임라인에 들어가는 Panel
    private GameObject monsterPanel;
    private GameObject mainCamera;

    [Header("Global Volume")]
    [SerializeField]private GameObject glitchVolume;

    //타임 라인 재생 중이면 발소리가 안나도록
    //FootstepSound 스크립트에서 사용할 수 있도록 static 플래그 변수 선언
    public static bool IsGameoverTimelinePlaying = false;

    void Start()
    {
        if (timeline != null)
            timeline.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector obj)
    {
        // 타임 라인 끝남
        DeadGameover.IsGameoverTimelinePlaying = false;

        // StartCoroutine(CallReSpawn());
        // ✅ GameManager에게 리스폰 실행을 위임합니다.
        if (GameManager.instance != null)
        {
            // 리스폰에 필요한 모든 정보를 GameManager로 전달합니다.
            GameManager.instance.StartRespawnRoutine(
                playerRespwan, 
                playerObject, 
                mainCamera, 
                monsterPanel, 
                gameObject.GetComponent<NavMeshAgent>(),
                gameObject.GetComponent<CapsuleCollider>()
            );
        
            // 몬스터의 hasPlayed 상태를 리셋할 필요 없이,
            // GameManager 코루틴이 끝난 후 (안전하게) static hasPlayed를 true로 리셋할 것입니다.
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            hasPlayed = false;

            //타임 라인 시작
            DeadGameover.IsGameoverTimelinePlaying = true;

            gameObject.GetComponent<CapsuleCollider>().enabled = false; // Collider 비활성화 추가 실행 방지
            gameObject.GetComponent<NavMeshAgent>().isStopped = true; //몬스터들 이동 멈추기 
            
            Debug.Log(gameObject.name);
            if(playerObject != null)
             playerObject.SetActive(false); // 플레이어 잠시 비활성화
            
            if (mainCamera != null) // OnTriggerEnter 들어올때 다시 트랙 바인딩
                SetTrackBinding(mainCamera, monsterPanel);
            
            // Timeline 실행
            if (timeline != null)
            {
                timeline.Play();
            }
        }
    }
    /*private IEnumerator CallReSpawn()
    {
        
        Debug.Log("CallReSpawn 코루틴 시작");
        yield return new WaitForSeconds(0.2f); // 타임라인이 끝나고 안정화 때까지 대기

        if (playerRespwan != null && playerObject != null && GameManager.instance != null)
        {
            // 1. Cinemachine Brain 참조 및 원본 Blend Time 저장
            CinemachineBrain brain = mainCamera.GetComponent<CinemachineBrain>();
            float originalBlendTime = 0f;
        
            if (brain != null)
            {
                // 블렌드 시간을 0으로 설정하여 즉시 전환(Cut)을 강제합니다.
                originalBlendTime = brain.DefaultBlend.Time;
                brain.DefaultBlend.Time = 0f;
            }
            monsterPanel.SetActive(false); // 빨간색
            
            yield return new WaitForSeconds(0.1f); // 몬스터 상태 리셋 지연

            hasPlayed = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = true; // 콜라이더 활성화
            gameObject.GetComponent<NavMeshAgent>().isStopped = false; // 이동 재개
            
            GameManager.instance.PlayerReSpawn(playerRespwan, playerObject); // 리스폰 
            playerObject.SetActive(true);
            
            // 원본 Blend Time 복구 및 Volume 복구
            if (brain != null)
            {
                // 카메라 전환이 완료될 시간을 주기 위해 다음 프레임을 기다립니다.
                yield return null; 
                // 원래 블렌드 시간으로 복구합니다.
                brain.DefaultBlend.Time = originalBlendTime;
            }
        }
    }*/

    public void SetPlayerWithUi(GameObject playerObject)
    {
        this.playerObject = playerObject;
    }

    public void SetTrackBinding(GameObject mainCamera, GameObject monsterPanel)
    {
        this.mainCamera = mainCamera;
        this.monsterPanel = monsterPanel;
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
                else if (track is ActivationTrack && track.name.Contains("Panel"))
                {
                    timeline.SetGenericBinding(track, monsterPanel);
                }
                else if (track is ActivationTrack && track.name.Contains("Glitch Effect"))
                {
                    timeline.SetGenericBinding(track, glitchVolume);
                }
            }
        }
    }
}