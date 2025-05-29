using UnityEngine;

public class Letter : MonoBehaviour
{
    [SerializeField] private Transform player; // 플레이어 트랜스폼
    [SerializeField] private Transform letterObject; // 편지 오브젝트 위치 (예: Newspapers_01)
    public float interactionDistance = 3f; // 상호작용 거리

    [SerializeField] private GameObject letterImage; // 편지 UI 

    private bool isInRange = false; // 플레이어가 범위 내에 있는지 여부
    private bool isReading = false; // 편지 UI가 열려있는지 여부

    void Update()
    {
        // 플레이어와 편지 오브젝트 거리 계산
        float dist = Vector3.Distance(player.position, letterObject.position);
        isInRange = dist <= interactionDistance;

        // E 키 입력으로 편지 열기
        if (isInRange && !isReading && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(DelayOpenLetter());
        }

        // E 또는 esc 키 입력으로 편지 닫기
        if (isReading && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CloseLetter();
        }
    }

    void OpenLetter()
    {

        letterImage.SetActive(true);
      

        isReading = true;
    }

    void CloseLetter()
    {
        letterImage.SetActive(false); // UI 숨김
        isReading = false;
    }

    private System.Collections.IEnumerator DelayOpenLetter()
    {
        yield return null; // 한 프레임 대기
        OpenLetter();
    }
}