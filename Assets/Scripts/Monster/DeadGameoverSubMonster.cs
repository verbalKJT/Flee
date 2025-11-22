using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DeadGameoverSubMonster : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;

    private GameObject playerObject;
    private GameObject mainCamera;
    private GameObject monsterPanel;
    private GameObject playerCameraHolder;

    private PlayerMouseLook playerMouseLook;

    private CinemachineBrain brain;

    private bool hasPlayed = true;

    private Animator monsterAnimator;

    // 천장 LayerMask (CeilingTest 기능)
    [SerializeField] private LayerMask ceilingMask;


    void Start()
    {
        if (timeline != null)
            timeline.stopped += OnTimelineFinished;

        brain = FindObjectOfType<CinemachineBrain>();
        monsterAnimator = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed) return;
        if (!other.CompareTag("Player")) return;

        hasPlayed = false;

        // Animator 즉시 비활성화 (목 애니메이션 덮어쓰기 방지)
        if (monsterAnimator != null)
            monsterAnimator.enabled = false;

        // NavMesh 멈춤
        var agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        // 충돌 off
        GetComponent<Collider>().enabled = false;

        // 1단계: 잡힌 순간 몬스터를 플레이어 위 천장으로 이동
        AttachToCeiling(other.transform);

        // 2단계: Timeline 실행
        StartCoroutine(StartTimeline());
    }

    private void AttachToCeiling(Transform player)
    {
        Debug.Log("AttachToCeiling 실행됨");
        Vector3 origin = player.position + Vector3.up * 1f;

        if (Physics.Raycast(origin, Vector3.up, out RaycastHit hit, 30f, ceilingMask))
        {
            // 바닥일 경우 무시 (y값이 player보다 낮으면 바닥)
            if (hit.point.y < player.position.y)
                return;

            transform.position = hit.point;
            transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
    }

    private IEnumerator StartTimeline()
    {
        yield return new WaitForSeconds(0.5f);

        SetTrackBinding(mainCamera, monsterPanel);

        // MouseLook 잠금
        if (playerMouseLook != null)
            playerMouseLook.enabled = false;

        timeline.stopped += OnTimelineEnd;
        timeline.Play();
    }


    private void OnTimelineFinished(PlayableDirector obj)
    {
        StartCoroutine(ReturnAndDestroy());
    }


    private IEnumerator ReturnAndDestroy()
    {
        yield return new WaitForSeconds(0.05f);

        if (monsterPanel != null)
            monsterPanel.SetActive(false);

        // 플레이어 카메라 복구
        Transform camHolder = playerObject.transform.Find("CameraHolder/PlayerCam");
        var playerCam = camHolder?.GetComponent<CinemachineVirtualCameraBase>();

        if (playerCam != null)
        {
            playerCam.Priority = 20;

            foreach (var cam in Resources.FindObjectsOfTypeAll<CinemachineVirtualCameraBase>())
            {
                if (cam != playerCam)
                    cam.Priority = 0;
            }

            yield return null;
            yield return null;

            if (brain != null)
            {
                float origin = brain.DefaultBlend.Time;
                brain.DefaultBlend.Time = 0f;
                yield return null;
                brain.DefaultBlend.Time = origin;
            }
        }

        Destroy(gameObject);
    }



    public void SetPlayerWithUi(GameObject playerObject)
    {
        this.playerObject = playerObject;

        playerMouseLook = playerObject.GetComponent<PlayerMouseLook>();
    }

    public void SetPlayerCameraHolder(GameObject holder)
    {
        this.playerCameraHolder = holder;
    }

    public void SetTrackBinding(GameObject mainCamera, GameObject monsterPanel)
    {
        this.mainCamera = mainCamera;
        this.monsterPanel = monsterPanel;

        TimelineAsset asset = timeline.playableAsset as TimelineAsset;
        if (asset == null) return;

        foreach (TrackAsset track in asset.GetOutputTracks())
        {
            if (track is CinemachineTrack)
            {
                var brain = mainCamera.GetComponent<CinemachineBrain>();
                timeline.SetGenericBinding(track, brain);
            }
            else if (track is ActivationTrack && track.name.Contains("Panel"))
            {
                timeline.SetGenericBinding(track, monsterPanel);
            }
            else if (track is AnimationTrack && track.name.Contains("CameraHolder"))
            {
                timeline.SetGenericBinding(track, playerCameraHolder);
            }
        }
    }
    private void OnTimelineEnd(PlayableDirector director)
    {
        // MouseLook 다시 활성화
        if (playerMouseLook != null)
            playerMouseLook.enabled = true;

        timeline.stopped -= OnTimelineEnd;
    }
}
