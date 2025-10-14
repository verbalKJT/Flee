using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Target (보통 CameraHolder)")]
    public Transform bobTarget;          // 비우면 this.transform 사용

    [Header("General")]
    public float minSpeedToBob = 0.15f;   // 이 속도 이상일 때만 흔들림
    public float returnSpeed = 10f;       // 멈출 때 원위치 복귀 속도
    public bool groundedOnly = true;

    [Header("Walk")]
    public float walkFreq = 1.8f;        // 빈도(Hz)
    public float walkAmpY = 0.04f;       // 상하 진폭(m)
    public float walkAmpX = 0f;      // 좌우 흔들림(옵션)

    [Header("Run Multiplier")]
    public float runFreqMul = 1.4f;
    public float runAmpMul = 1.3f;

    // 속도/지면 여부를 자동으로 가져오기(있으면 사용)
    CharacterController cc;
    Rigidbody rb;

    Vector3 baseLocalPos;
    float phase;

    void Awake()
    {
        if (bobTarget == null) bobTarget = transform;
        baseLocalPos = bobTarget.localPosition;
        cc = GetComponentInParent<CharacterController>();
        rb = GetComponentInParent<Rigidbody>();
    }

    float GetHorizontalSpeed()
    {
        if (cc != null) return cc.velocity.magnitude;
        if (rb != null)
        {
            Vector3 v = rb.linearVelocity; v.y = 0f; return v.magnitude;
        }
        // 없으면 Transform 기반(권장X)
        return 0f;
    }

    bool IsGrounded()
    {
        if (cc != null) return cc.isGrounded;
        if (rb != null) return true; // 필요하면 별도 Ground 체크와 연결
        return true;
    }

    void Update()
    {
        float speed = GetHorizontalSpeed();
        bool canBob = speed > minSpeedToBob && (!groundedOnly || IsGrounded());

        if (canBob)
        {
            // 달리기 가정: speed가 일정 기준 넘으면 런 멀티 적용(원하면 조건 수정)
            bool running = speed > 6.0f;
            float f = walkFreq * (running ? runFreqMul : 1f);
            float ay = walkAmpY * (running ? runAmpMul : 1f);
            float ax = walkAmpX * (running ? runAmpMul : 1f);

            phase += f * Time.deltaTime;

            // 2) 속도에 따라 진폭만 보간
            float speed01 = Mathf.InverseLerp(0f, 7f, speed);   // 7 = 달리기 속도
            float ampMul = Mathf.Lerp(0.6f, running ? runAmpMul : 1f, speed01);

            // 3) 진폭 적용
            float y = Mathf.Sin(phase * 2f * Mathf.PI) * ay * ampMul;
            float x = Mathf.Sin(phase * 4f * Mathf.PI) * ax * ampMul;

            Vector3 target = baseLocalPos + new Vector3(x, y, 0f);
            bobTarget.localPosition = Vector3.Lerp(bobTarget.localPosition, target, Time.deltaTime * returnSpeed);
        }
        else
        {
            bobTarget.localPosition = Vector3.Lerp(bobTarget.localPosition, baseLocalPos, Time.deltaTime * returnSpeed);
            // 정지 시 위상 리셋(원하면 주석)
            phase = 0f;
        }
    }
}