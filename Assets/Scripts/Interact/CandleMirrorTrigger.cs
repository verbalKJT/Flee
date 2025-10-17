using UnityEngine;

public class CandleMirrorTrigger : MonoBehaviour, IInteractable
{
    [Header("개별 촛불 설정")]
    public GameObject flameObject;             // 촛불의 불꽃 오브젝트

    [Header("공유 거울 오브젝트")]
    public GameObject[] mirrorObjects;         // 모든 촛불이 켜지면 보여줄 거울들
    
    [Header("공유 오디오 오브젝트")]
    public AudioSource allCandlesLitSFX;       // 모든 촛불이 켜졌을 때 재생할 효과음

    private bool isLit = false;                // 이 촛불이 켜졌는지 여부
    private float messageTimer = 0f;           // 메시지 표시 타이머
    private string currentMessage = "";        // 현재 표시할 상호작용 문구
    private static bool hasPlayedSFX = false;  // 오디오 중복 재생 방지

    public void Interact()
    {
        // 이미 켜졌으면 무시
        if (isLit) return;

        // 라이터가 있으면 촛불 켜기
        if (FindLighterInBox.hasLighter)
        {
            if (flameObject != null)
                flameObject.SetActive(true);  

            isLit = true;
            currentMessage = "";

            // 모든 촛불이 켜졌으면 거울 활성화
            if (AllCandlesLit())
            {
                foreach (var mirror in mirrorObjects)
                {
                    if (mirror != null)
                        mirror.SetActive(true);
                }
                // 효과음 재생 (한 번만)
                if (!hasPlayedSFX && allCandlesLitSFX != null)
                {
                    allCandlesLitSFX.Play();
                    hasPlayedSFX = true;
                }
            }
        }
        else
        {
            // 라이터 없으면 에러 메시지 표시
            currentMessage = "라이터가 없어 촛불을 켤 수 없습니다.";
            messageTimer = 1f;
        }
    }

    public string GetPromptText()
    {
        // 이미 켜진 촛불이면 문구 없음
        if (isLit) return "";

        // 일정 시간 동안 에러 메시지 유지
        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            return currentMessage;
        }

        return "[E] 촛불 키기"; // 기본 상호작용 문구
    }

    private bool AllCandlesLit()
    {
        // 모든 촛불이 켜졌는지 확인
        CandleMirrorTrigger[] allCandles = FindObjectsOfType<CandleMirrorTrigger>();
        foreach (var candle in allCandles)
        {
            if (!candle.isLit)
                return false;
        }
        return true;
    }
}
