using UnityEngine;
using UnityEngine.Audio;

public class WaterManager : MonoBehaviour
{
    [Header("초당 상승 속도")]
    public float riseSpeed = 0.5f;
    [Header(" 최대 높이")]
    public float maxHeight = 5f;
    [Header("Water_Cam 할당")]
    public GameObject cameraOverlayObject;

    public bool InWater;
    private float startY;
    private bool rising = false;      // 상승 중 여부

    public AudioClip underwaterClip;
    private AudioSource audioSource;

    void Start()
    {
        startY = transform.position.y;

        // 오디오소스 가져오기 또는 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // 천천히 위로 이동
        if (rising && transform.position.y < maxHeight)
        {            
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        }
    }

    
    /// 외부에서 물 차오름을 시작하게 할 때 호출
    public void StartRising()
    {
        rising = true;
        Debug.Log("Water rising started");
    }    
    /// 외부에서 물 차오름을 멈추게 할 수도 있음 (선택)
    public void StopRising()
    {
        rising = false;
        Debug.Log("Water rising stopped");
    }

    /// 트리거 진입
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //플래그 true
            InWater = true;
            Debug.Log("플레이어 물에 닿음");

            //플레이어 카메라효과            
            if ( cameraOverlayObject != null)
            {
                cameraOverlayObject.SetActive(true);                
            }

            if (underwaterClip != null && audioSource != null)
            {
                audioSource.clip = underwaterClip;
                audioSource.loop = true;
                audioSource.Play();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit 호출됨: " + other.name);
        if (other.CompareTag("Player"))
        {
            InWater = false;
            Debug.Log("플레이어 물에서 나옴");

            // 카메라 오버레이 해제
            if (cameraOverlayObject != null)
            {
                cameraOverlayObject.SetActive(false);
            }

            // 사운드 정지
            if (audioSource != null && audioSource.isPlaying)
            {
                Debug.Log("사운드 정지정지"+ audioSource+ audioSource.isPlaying);
                audioSource.Stop();
                audioSource.clip = null;
                audioSource.loop = false;
                Debug.Log("물 사운드 정지됨");
            }
        }
    }

    public void setCameraOverlay(GameObject overlayObj)
    {
        this.cameraOverlayObject= overlayObj;
    }
 }
