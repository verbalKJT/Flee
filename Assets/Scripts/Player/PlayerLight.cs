using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{   
    //플레이어 프리펩의 손전등 Light 오브젝트 연결
    [SerializeField] private Light flashlightLight; 
    private bool isOn = false;

    public AudioSource lightAudioSource;
    public AudioClip lightOnClip;
    public AudioClip lightOffClip;

    private PlayerLightInventory inv;

    // ==========================
    //  과열/깜빡임 관련 설정
    // ==========================
    [Header("과열 / 깜빡임 설정")]
    [Tooltip("손전등을 연속으로 켜두는 시간(초). 이 시간을 넘기면 깜빡이기 시작.")]
    public float overheatTime = 20f;

    [Tooltip("손전등을 끄고 있는 시간(초). 이 시간만큼 꺼두면 과열 상태가 해제됨.")]
    public float cooldownTime = 3f;

    [Tooltip("깜빡임 최소 간격(초)")]
    public float flickerMinInterval = 0.05f;

    [Tooltip("깜빡임 최대 간격(초)")]
    public float flickerMaxInterval = 0.2f;

    [Tooltip("깜빡이는 동안 기본 밝기의 몇 배로 유지할지 (0.3 = 30% 밝기)")]
    public float flickerIntensityMultiplier = 0.3f;

    private float baseIntensity = 1f;
    private float onTimer = 0f;      // 켜져있는 시간(연속)
    private float offTimer = 0f;     // 꺼져있는 시간(연속)

    private bool isFlickering = false;
    private float flickerTimer = 0f;
    private float nextFlickerTime = 0.1f;
    private bool flickerLightEnabledState = true;

    // 외부에서 상태 확인용
    public bool IsOn => isOn;
    public bool IsFlickering => isFlickering;
    void Awake()
    {
        inv = GetComponent<PlayerLightInventory>();

        //인스펙터에 연결 안했으면 자동으로 찾기
        if (flashlightLight == null)
            // 자식까지 전부 뒤져서 Light 하나 찾아옴
            flashlightLight = GetComponentInChildren<Light>(true);

        if (flashlightLight != null)
        {
            baseIntensity = flashlightLight.intensity; //현재 라이트 밝기를 기준값으로 저장
            flashlightLight.enabled = false;   // 처음엔 꺼진 상태
            
        }
    }

    void Update()
    {
        if (inv == null || flashlightLight == null) return;
        if (!inv.hasFlashlight) return;

        if (Input.GetKeyDown(KeyCode.F) || OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            ToggleFlashlight();
        }
        //손전등 켜진 시간에 따라 깜박이도록 하는 메서드
        HandleOverheatAndFlicker();

        Debug.Log("손전등 켜진 시간: " + onTimer);
    }

    private void ToggleFlashlight()
    {
            isOn = !isOn;

            if (!isOn)
            {
                // 끌 때는 과열/깜빡임 관련 상태 초기화용 타이머 돌리기
                offTimer = 0f;
            }

            // 기본 온/오프
            flashlightLight.enabled = isOn && !isFlickering; // 깜빡임 중이면 flicker 로직이 제어

            // 사운드 재생
            if (lightAudioSource != null && lightOnClip != null && lightOffClip != null)
            {
                lightAudioSource.PlayOneShot(isOn ? lightOnClip : lightOffClip);
            }
    }

    private void HandleOverheatAndFlicker()
    {
        if (isOn)
        {
            // 켜져있을 때: onTimer 증가, offTimer 리셋
            onTimer += Time.deltaTime;
            offTimer = 0f;

            // 아직 깜빡임이 아니라면, 과열 시간 체크
            if (!isFlickering && onTimer >= overheatTime)
            {
                StartFlicker();
            }
        }
        else
        {
            // 꺼져있을 때: offTimer 증가, onTimer 리셋
            offTimer += Time.deltaTime;

            // 꺼진 상태로 충분히 있으면 과열/깜빡임 완전 해제
            if (offTimer >= cooldownTime)
            {
                onTimer = 0f;
                if (isFlickering)
                    StopFlicker();
            }
        }

        // 실제 깜빡임 효과 적용
        if (isFlickering && isOn)
        {
            UpdateFlicker();
        }
        else if (flashlightLight.enabled)
        {
            // 정상 상태일 때는 항상 원래 밝기 유지
            flashlightLight.intensity = baseIntensity;
        }
    }

    private void StartFlicker()
    {
        isFlickering = true;
        flickerTimer = 0f;
        nextFlickerTime = Random.Range(flickerMinInterval, flickerMaxInterval);
        flickerLightEnabledState = true;

        // 처음 깜빡임 진입 시, 조금 더 어두운 상태로 시작
        flashlightLight.enabled = true;
        flashlightLight.intensity = baseIntensity * flickerIntensityMultiplier;
    }

    private void StopFlicker()
    {
        isFlickering = false;

        // 깜빡임 해제 후, 손전등이 켜져 있다면 원래 상태로 복구
        if (isOn)
        {
            flashlightLight.enabled = true;
            flashlightLight.intensity = baseIntensity;
        }
        else
        {
            flashlightLight.enabled = false;
        }
    }

    /// 깜빡임 동안 불빛을 약하게 + 랜덤 on/off
    private void UpdateFlicker()
    {
        flickerTimer += Time.deltaTime;

        if (flickerTimer >= nextFlickerTime)
        {
            // on/off 토글
            flickerLightEnabledState = !flickerLightEnabledState;
            flashlightLight.enabled = flickerLightEnabledState;

            flickerTimer = 0f;
            nextFlickerTime = Random.Range(flickerMinInterval, flickerMaxInterval);
        }

        if (flashlightLight.enabled)
        {
            // 기본 밝기의 일정 비율로 유지하면서 약간의 노이즈
            float noise = Random.Range(-0.1f, 0.1f);
            float mul = Mathf.Clamp(flickerIntensityMultiplier + noise, 0.1f, flickerIntensityMultiplier + 0.2f);
            flashlightLight.intensity = baseIntensity * mul;
        }
    }
}
