using UnityEngine;

public class Letter : MonoBehaviour
{
    [SerializeField] private Transform player;              // 플레이어 트랜스폼
    [SerializeField] private Transform letterObject;        // 편지 오브젝트 위치 (예: Newspapers_01)
    public float interactionDistance = 3f; // 상호작용 거리

    [SerializeField] private GameObject letterPanel;        // 편지 UI 패널

    private bool isInRange = false; // 플레이어가 범위 내에 있는지 여부
    private bool isReading = false; // 편지 UI가 열려있는지 여부

    void Update()
    {
        // 플레이어와 편지 오브젝트 거리 계산
        float dist = Vector3.Distance(player.position, letterObject.position);
        isInRange = dist < interactionDistance;

        // E 키 입력으로 편지 열기
        if (isInRange && !isReading && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("편지 열기 시도됨");
            OpenLetter();
        }

        // E 또는 esc 키 입력으로 편지 닫기
        if (isReading && (Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.Escape)))
        {
            CloseLetter();
        }
    }

    void OpenLetter()
    {
        Debug.Log("OpenLetter 호출");

        if (letterPanel == null)
        {
            Debug.LogError("❌ letterPanel 연결 안됨");
            return;
        }

        letterPanel.SetActive(true);
        Debug.Log("✅ letterPanel 활성화됨? " + letterPanel.activeInHierarchy);

        var image = letterPanel.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            Debug.Log("🎨 이미지 알파값: " + image.color.a);
        }

        isReading = true;
    }

    void CloseLetter()
    {
        letterPanel.SetActive(false); // UI 숨김
        isReading = false;
    }
}
