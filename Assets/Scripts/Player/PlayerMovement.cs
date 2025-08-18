using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 1.5f;  //걷기 속도
    public float runSpeed = 3f;     //달리기 속도

    private CharacterController controller;
    private Animator animator;

    private bool canMove = true;

    private PlayerStamina stamina;      //플레이어 스태미나

    [Header("Water_Object할당")]
    public WaterManager InWater;
    [Header("속도 계수")]
    public float speedMultiplier = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        stamina = GetComponent<PlayerStamina>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove || FindObjectOfType<PlayerHider>().IsHiding)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");      //앞뒤 이동 키 입력(w,s)
        float v = Input.GetAxis("Vertical");        //양옆 이동 키 입력(a,d)

        Vector3 input = new Vector3(h, 0, v);
        Vector3 move = transform.TransformDirection(input.normalized);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);       //달리기 키 입력(LShift)

        //스태미나가 0이하면 달리지 못하도록 설정
        if (!stamina.CanRun)
            isRunning = false;

        stamina.isRunning = isRunning;   //스태미나 조절을 위해 전달

        
        //InWater조건에 따라 속도계수 조정
        float speedFactor = 1f;
        if (InWater != null && InWater.InWater)
        {
            speedFactor = speedMultiplier;
        }

        float currentSpeed = (isRunning ? runSpeed : walkSpeed) * speedFactor;
       // Debug.Log("현재속도" + currentSpeed);

        controller.SimpleMove(move * currentSpeed);

        // 이동 속도를 기반으로 애니메이션 전이
        animator.SetFloat("Speed", input.magnitude * currentSpeed);
        if(currentSpeed > 0)
        {
            animator.SetBool("isRunning", isRunning);
        }
            
    }
}
