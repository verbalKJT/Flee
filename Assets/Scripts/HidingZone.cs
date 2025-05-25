using UnityEngine;

public class HidingZone : MonoBehaviour
{
    public bool isQTERequired = true;   //QTE�� �ʿ�����
    public Transform hidingSpot;        //�÷��̾� ĳ���Ͱ� ���� ��ġ

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //null ���ǿ�����, PlyaerHider��ü�� ���� �ƴҶ��� �޼ҵ� ����
            other.GetComponent<PlayerHider>()?.EnterHidingZone(this);   
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //null ���ǿ�����, PlyaerHider��ü�� ���� �ƴҶ��� �޼ҵ� ����
            other.GetComponent<PlayerHider>()?.ExitHidingZone();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
