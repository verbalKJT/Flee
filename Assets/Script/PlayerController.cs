using System;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    public Transform cameraTransform;
    public float speed;

    public float jumpForce = 5f;

    private bool isGrounded;
    public Transform groundCheck;          // 발 아래 위치한 빈 오브젝트
    public float groundCheckRadius = 0.3f; // 체크 범위
    public LayerMask groundMask;           // 바닥 레이어

    public float fallMultiplier ;

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
       isGrounded=IsGrounded();
       Move();
       Jump();
    }

    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");        

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        Vector3 move = (camForward * zInput + camRight * xInput) * speed;
        playerRigidbody.linearVelocity = move;

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            playerRigidbody.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
        }
        if (playerRigidbody.angularVelocity.y < 0f)
        {
            playerRigidbody.linearVelocity+=Vector3.up*Physics.gravity.y* (fallMultiplier - 1f) * Time.deltaTime;
        }
    }

    bool IsGrounded()
    {
        Debug.Log("IsGrounded: " + isGrounded);
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    }

}
