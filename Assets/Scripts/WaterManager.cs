using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [Header("초당 상승 속도")]
    public float riseSpeed = 0.5f;
    [Header(" 최대 높이")]
    public float maxHeight = 5f;
    [Header("Water_Cam 할당")]
    public GameObject cameraOverlayObject;

    private float startY;
    private bool rising = false;      // 상승 중 여부

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (rising && transform.position.y < maxHeight)
        {
            // 천천히 위로 이동
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

    /// 트리거 진입 시 로그 출력 (테스트용)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 물에 닿음");

            //플레이어 이동
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 플레이어를 부드럽게 위로 이동시킴
                Vector3 targetPosition = rb.position + Vector3.up * riseSpeed * Time.deltaTime;

                // MovePosition으로 자연스러운 이동 유도
                rb.MovePosition(targetPosition);
            }

            //플레이어 카메라효과
            
            if ( cameraOverlayObject != null)
            {
                cameraOverlayObject.SetActive(true);
                Debug.Log(" 물 시야 오버레이 활성화됨");
            }
        }
    }
}
