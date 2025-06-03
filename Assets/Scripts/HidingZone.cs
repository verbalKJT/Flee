using UnityEngine;

public class HidingZone : MonoBehaviour
{
    public bool isQTERequired = true;   //QTE가 필요한지
    public Transform hidingSpot;        //플레이어 캐릭터가 숨는 위치
	
	
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //null 조건연산자, PlyaerHider객체가 널이 아닐때만 메소드 실행
            other.GetComponent<PlayerHider>()?.EnterHidingZone(this);   
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //null 조건연산자, PlyaerHider객체가 널이 아닐때만 메소드 실행
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
