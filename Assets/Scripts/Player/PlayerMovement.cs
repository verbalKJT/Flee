using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 1.5f; //걷기 속도
    public float runSpeed = 3f; //달리기 속도

    [Header("Water_Object할당")] public WaterManager InWater;

    [Header("속도 계수")] public float speedMultiplier = 0.5f;

    [Header("Footstep Audio")] public AudioSource footstepSource;

    public AudioClip[] footstepClips;

    [Header("텔레포트 위치")] [SerializeField] private Transform middlCorriderPos;
    [Header("텔레포트 위치")] [SerializeField] private Transform lastCorriderPos;
    [Header("텔레포트 위치")] [SerializeField] private Transform endRoomPos;

    private Animator animator;

    private bool canMove;

    private CharacterController controller;

    private PlayerStamina stamina; //플레이어 스태미나

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        stamina = GetComponent<PlayerStamina>();
        // 3초 후 플레이어 움직임 적용 시킴 -> 맵 생성 중에 맵 바깥으로 떨어지지 않도록.
        StartCoroutine(EnableMoveDelay());
    }

    // Update is called once per frame
    private void Update()
    {
        if (!canMove || FindObjectOfType<PlayerHider>().IsHiding) return;

        var h = Input.GetAxis("Horizontal"); //앞뒤 이동 키 입력(w,s)
        var v = Input.GetAxis("Vertical"); //양옆 이동 키 입력(a,d)

        var input = new Vector3(h, 0, v);
        var move = transform.TransformDirection(input.normalized);

        //달리기 키 입력(LShift)
        var isRunning = Input.GetKey(KeyCode.LeftShift) ||
                        ARAVRInput.Get(ARAVRInput.Button.Two);

        //스태미나가 0이하면 달리지 못하도록 설정
        if (!stamina.CanRun)
            isRunning = false;

        stamina.isRunning = isRunning; //스태미나 조절을 위해 전달


        //InWater조건에 따라 속도계수 조정
        var speedFactor = 1f;
        if (InWater != null && InWater.InWater) speedFactor = speedMultiplier;

        var currentSpeed = (isRunning ? runSpeed : walkSpeed) * speedFactor;
        // Debug.Log("현재속도" + currentSpeed);

        controller.SimpleMove(move * currentSpeed);

        // 이동 속도를 기반으로 애니메이션 전이
        animator.SetFloat("Speed", input.magnitude * currentSpeed);
        if (currentSpeed > 0) animator.SetBool("isRunning", isRunning);
        // 텔레포트 -> 빌드 시 삭제 필요
        // 오른쪽 인덱스 트리거 -> 중간 복도
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            transform.position = middlCorriderPos.transform.position;
        } // 왼쪽 핸드트리거 시 마지막 복도 시작 부분
        else if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            transform.position = lastCorriderPos.transform.position;
        }// 왼쪽 인덱스 트리거 시 엔딩룸 안
        else if(ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch))
        {
            transform.position = endRoomPos.transform.position;
        }
            
    }

    private IEnumerator EnableMoveDelay()
    {
        yield return new WaitForSeconds(3f);
        canMove = true;
    }

    // 애니메이션 이벤트에서 호출
    public void Footstep()
    {
        if (footstepClips.Length == 0 || footstepSource == null)
            return;

        var index = Random.Range(0, footstepClips.Length);
        footstepSource.PlayOneShot(footstepClips[index]);
    }

    public void SetTeleportPoint(Transform middlCorriderPos, Transform lastCorriderPos, Transform endRoomPos)
    {
        this.middlCorriderPos = middlCorriderPos;
        this.lastCorriderPos = lastCorriderPos;
        this.endRoomPos = endRoomPos;
    }
}