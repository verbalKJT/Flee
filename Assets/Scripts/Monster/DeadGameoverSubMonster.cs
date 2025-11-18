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

    private CinemachineBrain brain;

    private bool hasPlayed = true;

    void Start()
    {
        if (timeline != null)
            timeline.stopped += OnTimelineFinished;

        brain = FindObjectOfType<CinemachineBrain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed) return;
        if (!other.CompareTag("Player")) return;

        hasPlayed = false;

        // 몬스터 이동/충돌 정지
        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<Collider>().enabled = false;

        StartCoroutine(StartTimeline());
    }

    private IEnumerator StartTimeline()
    {
        // 3초 딜레이 후 타임라인 재생
        yield return new WaitForSeconds(3f);

        SetTrackBinding(mainCamera, monsterPanel);
        timeline.Play();
    }

    private void OnTimelineFinished(PlayableDirector obj)
    {
        StartCoroutine(ReturnAndDestroy());
    }

    private IEnumerator ReturnAndDestroy()
    {
        // 안정화 없으면 겹침 오류남
        yield return new WaitForSeconds(0.05f);

        if (monsterPanel != null)
            monsterPanel.SetActive(false);

        // PlayerCam 확보
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

            // 화면 떨림 최소화를 위해 BlendTime만 잠시 0으로
            if (brain != null)
            {
                float origin = brain.DefaultBlend.Time;
                brain.DefaultBlend.Time = 0f;
                yield return null;
                brain.DefaultBlend.Time = origin;
            }
        }

        // 몬스터 삭제
        Destroy(gameObject);
    }

    public void SetPlayerWithUi(GameObject playerObject)
    {
        this.playerObject = playerObject;
    }

    // Timeline Track 바인딩
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
        }
    }
}
