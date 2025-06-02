using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [Header("�ʴ� ��� �ӵ�")]
    public float riseSpeed = 0.5f;
    [Header(" �ִ� ����")]
    public float maxHeight = 5f;
    [Header("Water_Cam �Ҵ�")]
    public GameObject cameraOverlayObject;

    public bool InWater;
    private float startY;
    private bool rising = false;      // ��� �� ����

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        // õõ�� ���� �̵�
        if (rising && transform.position.y < maxHeight)
        {            
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        }
    }

    
    /// �ܺο��� �� �������� �����ϰ� �� �� ȣ��
    public void StartRising()
    {
        rising = true;
        Debug.Log("Water rising started");
    }    
    /// �ܺο��� �� �������� ���߰� �� ���� ���� (����)
    public void StopRising()
    {
        rising = false;
        Debug.Log("Water rising stopped");
    }

    /// Ʈ���� ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //�÷��� true
            InWater = true;
            Debug.Log("�÷��̾� ���� ����");

            //�÷��̾� �̵�
           /* Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // �÷��̾ �ε巴�� ���� �̵���Ŵ
                Vector3 targetPosition = rb.position + Vector3.up * riseSpeed * Time.deltaTime;

                // MovePosition���� �ڿ������� �̵� ����
                rb.MovePosition(targetPosition);
            }
           */
            //�÷��̾� ī�޶�ȿ��            
            if ( cameraOverlayObject != null)
            {
                cameraOverlayObject.SetActive(true);                
            }
                       
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit ȣ���: " + other.name);
        if (other.CompareTag("Player"))
        {
            InWater = false;
            Debug.Log("�÷��̾� ������ ����");

            // ī�޶� �������� ����
            if (cameraOverlayObject != null)
            {
                cameraOverlayObject.SetActive(false);                
            }            
        }
    }
 }
