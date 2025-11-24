using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Unity.Cinemachine;
using System.Collections;

public class DeadGameoverHolyway : MonoBehaviour
{
    [Header("Timeline and UI 설정")]
    public PlayableDirector timeline;
    public GameObject monsterPanel;
    public GameObject glitchVolume;

    [Header("Player 및 Respawn 자동 연결")]
    public GameObject playerObject;
    public Transform playerRespawn;

    private CinemachineBrain brain;
    private HolywayMonsterAI caughtMonster;
    private bool hasPlayed;

    private void Awake()
    {
        playerObject ??= GameObject.FindGameObjectWithTag("Player");
        FindRespawnPoint();
    }

    private void Start()
    {
        timeline.stopped += _ => StartCoroutine(HandlePostTimeline());
        brain = Camera.main?.GetComponent<CinemachineBrain>();
        StartCoroutine(UpdateRespawnAuto());
    }

    private IEnumerator UpdateRespawnAuto()
    {
        while (true)
        {
            FindRespawnPoint();
            yield return new WaitForSeconds(1f);
        }
    }

    private void FindRespawnPoint()
    {
        var holyway = GameObject.Find("holyway(Clone)") ?? GameObject.Find("holyway");
        if (holyway == null) return;
        var respawn = holyway.transform.Find("RespawnPoint");
        if (respawn != null) playerRespawn = respawn;
    }

    public void PlayDeathCutscene(HolywayMonsterAI monster)
    {
        if (hasPlayed || timeline == null) return;
        hasPlayed = true;
        caughtMonster = monster;

        // 여기에서 즉시 Player 찾기 (FailSafe)
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null)
            {
                return;
            }
        }

        // 몬스터 멈춤
        monster.GetComponent<NavMeshAgent>()?.SetDestination(monster.transform.position);
        monster.GetComponent<Animator>()?.SetFloat("Speed", 0f);

        // Player 비활성화
        playerObject.SetActive(false);

        SetTrackBindings();
        timeline.Play();
    }


    private IEnumerator HandlePostTimeline()
    {
        yield return new WaitForSeconds(0.3f);

        playerObject?.SetActive(true);
        if (caughtMonster) Destroy(caughtMonster.gameObject);
        EndCorridorGameManager.I?.ResetCorridor();

        if (playerRespawn && playerObject)
        {
            playerObject.transform.SetPositionAndRotation(playerRespawn.position, playerRespawn.rotation);
            playerObject.SetActive(true);
        }

        yield return StartCoroutine(ResetToPlayerCam());
        hasPlayed = false;
    }

    private IEnumerator ResetToPlayerCam()
    {
        yield return new WaitForSeconds(0.2f);

        var camHolder = playerObject?.transform.Find("CameraHolder/PlayerCam");
        var playerCam = camHolder?.GetComponent<CinemachineCamera>();
        if (playerCam == null) yield break;

        foreach (var cam in Resources.FindObjectsOfTypeAll<CinemachineCamera>())
            cam.Priority = (cam == playerCam) ? 20 : 0;

        yield return null;
        brain?.ManualUpdate();
    }

    private void SetTrackBindings()
    {
        if (timeline.playableAsset is not TimelineAsset asset) return;

        foreach (var track in asset.GetOutputTracks())
        {
            if (track is CinemachineTrack)
                timeline.SetGenericBinding(track, brain);
            else if (track is ActivationTrack)
            {
                if (track.name.Contains("Panel")) timeline.SetGenericBinding(track, monsterPanel);
                else if (track.name.Contains("Glitch")) timeline.SetGenericBinding(track, glitchVolume);
            }
        }
    }
}
