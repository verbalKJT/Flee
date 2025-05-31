using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [Header("�ʴ� ��� �ӵ�")]
    public float riseSpeed = 0.5f;
    [Header(" �ִ� ����")]
    public float maxHeight = 5f;
    [Header("Water_Cam �Ҵ�")]
    public GameObject cameraOverlayObject;

    private float startY;
    private bool rising = false;      // ��� �� ����

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (rising && transform.position.y < maxHeight)
        {
            // õõ�� ���� �̵�
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

    /// Ʈ���� ���� �� �α� ��� (�׽�Ʈ��)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ���� ����");

            //�÷��̾� �̵�
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // �÷��̾ �ε巴�� ���� �̵���Ŵ
                Vector3 targetPosition = rb.position + Vector3.up * riseSpeed * Time.deltaTime;

                // MovePosition���� �ڿ������� �̵� ����
                rb.MovePosition(targetPosition);
            }

            //�÷��̾� ī�޶�ȿ��
            
            if ( cameraOverlayObject != null)
            {
                cameraOverlayObject.SetActive(true);
                Debug.Log(" �� �þ� �������� Ȱ��ȭ��");
            }
        }
    }
}
