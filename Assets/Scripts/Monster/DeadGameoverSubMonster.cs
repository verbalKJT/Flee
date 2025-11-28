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
    private GameObject playerModel;
    private CinemachineInputAxisController inputAxis;  // PlayerCam의 Y 회전 제어
    private PlayerMovement playerMovement;             // Player의 이동 스크립트
    private Animator playerAnimator;
    private HeadBob headBob;

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
        monsterAnimator = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed) return;
        if (!other.CompareTag("Player")) return;

        hasPlayed = false;

        DisablePlayerControlsImmediately();

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
    {   // 플레이어 캠 위로 가게 변경
        Vector3 origin = player.transform.Find("CameraHolder/PlayerCam").position + Vector3.up * 1f;

        if (Physics.Raycast(origin, Vector3.up, out RaycastHit hit, 30f, ceilingMask))
        {
            // 바닥일 경우 무시 (y값이 player보다 낮으면 바닥)
            if (hit.point.y < player.position.y)
                return;
            // 서브몬스터 위치에서 플레이어캠에 더 가깝게
            Vector3 finalPos = hit.point;
            finalPos.x -= 1.0f;
            transform.position = finalPos;
            
            transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
    }

    private IEnumerator StartTimeline()
    {
        playerModel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        timeline.stopped += OnTimelineEnd;
        timeline.Play();
    }


    private void OnTimelineFinished(PlayableDirector obj)
    {
        // pc
        //StartCoroutine(ReturnAndDestroy());
    }

    public void DropToPlayerSignal(){
        // vr -> 서브몬이 플레이어를 향해 떨어지는 기믹으로 변경
        // 타임라인 시그널 트랙으로 호출
        StartCoroutine(DropToPlayer());
    }

/*
    private IEnumerator ReturnAndDestroy()
    {
        yield return new WaitForSeconds(0.05f);
        
        vr에서 시점 변경이 안되는 문제로 인해 기믹 변경 -> 아래 코드 필요 없음
        playerObject.transform.Find("CameraHolder/PlayerCam").gameObject.SetActive(true);
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
        
        // 서브몬스터가 플레이어 방향으로 떨어짐
        
        //Destroy(gameObject);
    } */

    private IEnumerator DropToPlayer()
    {
        if (monsterPanel != null)
            monsterPanel.SetActive(false);
        
        // NavMeshAgent 찾아서 끔
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        
        // rb 찾아서 끔
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // 물리연산 끄기
        

        // 콜라이더 다시 키기
        Collider col = GetComponent<Collider>();
        col.enabled = true;
        
        col.isTrigger = false; // OnCollisionEnter 사용하기 위해
        
        // 목표: PlayerCam
        Transform target = playerObject.transform.Find("CameraHolder/PlayerCam");
        
        if (target != null)
        {
            
            transform.LookAt(target,Vector3.down);
            
            transform.Rotate(90f,0f,0f,Space.Self);
            
            Vector3 finalPos = target.position + target.right*(-0.5f);
            
            // (목표 위치 - 내 위치)
            Vector3 direction = (finalPos - transform.position).normalized;
            
            // * dropPower 숫자 조절
            float dropPower = 4.0f;
            rb.AddForce(direction * dropPower, ForceMode.Impulse); // ForceMode.Impulse -> 순각적인 추진력
            yield return new WaitForSeconds(0.1f);
            transform.localScale = new Vector3(3f, 3f, 3f);
            
            // 애니메이션 변경 -> 리깅 할지
        }
        
        yield return new WaitForSeconds(1f); // 떨어질 시간    
        
        Destroy(gameObject);
    }

    public void SetPlayerWithUi(GameObject playerObject)
    {
        this.playerObject = playerObject;

        playerAnimator = playerObject.GetComponent<Animator>();
        playerMouseLook = playerObject.GetComponent<PlayerMouseLook>();
        playerModel = playerObject.transform.Find("SK_Changeling_Boy").gameObject;
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        inputAxis = playerObject.transform.Find("CameraHolder/PlayerCam")
             ?.GetComponent<CinemachineInputAxisController>();
        Transform cam = playerObject.transform.Find("CameraHolder/PlayerCam");
        if (cam != null)
            headBob = cam.GetComponent<HeadBob>();
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
            else if (track is AnimationTrack && track.name.Contains("Player"))
            {
                timeline.SetGenericBinding(track, playerObject); 
            }
        }
    }
    private void OnTimelineEnd(PlayableDirector director)
    {
        // MouseLook 다시 활성화
        if (playerMouseLook != null)
            playerMouseLook.enabled = true;

        // PlayerMovement 다시 활성화
        if (playerMovement != null)
            playerMovement.enabled = true;

        // Look X/Y 다시 활성화
        if (inputAxis != null)
            inputAxis.enabled = true;

        if (playerAnimator != null)
            playerAnimator.enabled = true;

        if (headBob != null)
        {
            headBob.enabled = true;
            Debug.Log("컷씬 종료 — HeadBob 다시 활성화");
        }

        playerModel.SetActive(true);

        timeline.stopped -= OnTimelineEnd;
    }
    private void DisablePlayerControlsImmediately()
    {
        if (headBob != null)
        {
            headBob.enabled = false;
            Debug.Log("컷씬 시작 — HeadBob 비활성화 완료");
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);   // Idle 파라미터
            //playerAnimator.enabled = false;         // 애니 완전 정지 -> vr에서 잠시 막기
        }

        // 1) PlayerMovement 끄기
        if (playerMovement != null)
            playerMovement.enabled = false;

        // 2) PlayerMouseLook 끄기
        if (playerMouseLook != null)
            playerMouseLook.enabled = false;

        // 3) Cinemachine 상하/좌우 끄기
        if (inputAxis != null)
            inputAxis.enabled = false;
    }
    
    
    // 물리적 충돌이 일어났을 때 실행되는 함수
    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어와 부딪혔다면
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                // (정지)
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // 물리 연산 제거, 제자리에 고정
                rb.isKinematic = true; 
            }
        }
    }
}
