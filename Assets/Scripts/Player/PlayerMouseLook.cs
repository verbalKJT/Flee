using UnityEngine;
using UnityEngine.InputSystem;   // 새 Input System

public class PlayerMouseLook : MonoBehaviour
{
    [Header("Refs")]
    public Transform playerBody;                       // 반드시 할당(보통 Player 루트)
    public InputActionReference lookAction;            // CM에서 쓰는 Look 액션과 동일한 것
    [Range(0.1f, 1000f)] public float yawSpeed = 2.5f; // 감도

    private InputAction _look;                         // 캐싱
    private bool _useNewInput;

    void Awake()
    {
        _useNewInput = (lookAction != null && lookAction.action != null);
        if (_useNewInput)
            _look = lookAction.action;
        if (playerBody == null)
            Debug.LogError("[PlayerMouseLook] playerBody가 비어 있습니다.");
    }

    void OnEnable()
    {
        _look?.Enable();   // null이면 호출 안 함
    }

    void OnDisable()
    {
        _look?.Disable();
    }

    void Update()
    {
        if (playerBody == null) return;

        float yawDelta;

        if (_useNewInput)
        {
            // 액션이 null이면 조용히 빠져나와 NRE 방지
            if (_look == null) return;
            Vector2 look = _look.ReadValue<Vector2>();   // (x=Yaw, y=Tilt)
            yawDelta = look.x * yawSpeed * Time.deltaTime;
        }
        else
        {
            // 구 InputManager 폴백
            float mx = Input.GetAxis("Mouse X");
            yawDelta = mx * yawSpeed * Time.deltaTime;
        }

        playerBody.Rotate(0f, yawDelta, 0f);   // Yaw만 적용
    }
}