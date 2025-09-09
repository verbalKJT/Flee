using UnityEngine;

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

    void Start()
    {
        startY = transform.position.y;
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
        }
    }
 }
