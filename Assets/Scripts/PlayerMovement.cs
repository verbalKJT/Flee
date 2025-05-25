using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 1.5f;  //�ȱ� �ӵ�
    public float runSpeed = 3f;     //�޸��� �ӵ�

    private CharacterController controller;
    private Animator animator;

    private bool canMove = true;

    private PlayerStamina stamina;      //�÷��̾� ���¹̳�

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

        float h = Input.GetAxis("Horizontal");      //�յ� �̵� Ű �Է�(w,s)
        float v = Input.GetAxis("Vertical");        //�翷 �̵� Ű �Է�(a,d)

        Vector3 input = new Vector3(h, 0, v);
        Vector3 move = transform.TransformDirection(input.normalized);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);       //�޸��� Ű �Է�(LShift)

        //���¹̳��� 0���ϸ� �޸��� ���ϵ��� ����
        if (!stamina.CanRun)
            isRunning = false;

        stamina.isRunning = isRunning;   //���¹̳� ������ ���� ����

        float currentSpeed = isRunning ? runSpeed : walkSpeed;  


        controller.SimpleMove(move * currentSpeed);

        // �̵� �ӵ��� ������� �ִϸ��̼� ����
        animator.SetFloat("Speed", input.magnitude * currentSpeed);
        if(currentSpeed > 0)
        {
            animator.SetBool("isRunning", isRunning);
        }
            
    }
}
