using System.Collections;
using UnityEngine;

public class Letter : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform letterObject; // 편지 오브젝트 위치 (예: Newspapers_01)
    private GameObject letterImage; // 편지 UI 
    private Animator doorAnimator; // StartingRoom Animator
    private bool isReading = false; // 편지 UI가 열려있는지 여부
    private bool isOpenDoor = false; // 문이 열려있는지
    // 🔥 책상 위에 있는 Candle_01 오브젝트 (MAP/Furniture/.../Candle_01)
    [SerializeField] private GameObject tableCandle;

    // Inspector 에선 비워두고 런타임에 찾는다
    [SerializeField] private string playerCandleLightName = "CandleLight";
    private GameObject playerCandleLight;
    private bool candleActivated = false;

    // 📌 IInteractable 구현: 상호작용 시 실행됨
    public void Interact()
    {
        if (!isReading)
        {
            StartCoroutine(DelayOpenLetter());
        }
        else
        {
            CloseLetter();
        }
    }

    public string GetPromptText()
    {
        return isReading ? "[A] 편지 닫기" : "[A] 편지 읽기";
    }
    void OpenLetter()
    {
        letterImage.SetActive(true);
        isReading = true;
        doorAnimator.SetTrigger("Open");
    }

    void CloseLetter()
    {
        letterImage.SetActive(false); // UI 숨김
        isReading = false;

        if(!candleActivated)
        {
            candleActivated = true;

            // 플레이어 프리펩에 존재하는 양초 불빛 오브젝트 찾기
            TryFindPlayerCandle();

            //책상 위의 촛불 제거(플레이어가 획득한 것처럼)
            if(tableCandle != null)
            {
                Destroy(tableCandle);
            }
            //플레이어 양초 불 활성화
            if (playerCandleLight != null)
            {
                Debug.Log("양초 불 활성화");
                playerCandleLight.SetActive(true);
            }
            else
            {
                Debug.Log("양초 불 활성화 못함!");
            }
        }
    }

    private IEnumerator DelayOpenLetter()
    {
        yield return null; // 한 프레임 대기
        OpenLetter();
    }
    public void SetupEnvironment(Animator doorAnimator, GameObject letterImage)
    {
        this.doorAnimator = doorAnimator;
        this.letterImage = letterImage;
        Debug.Log("Letter 환경 설정 완료");
    }
    void TryFindPlayerCandle()
    {
        if (playerCandleLight != null) return;

        // 1. Player 태그 달린 오브젝트 찾기 (Addressables 로드된 플레이어)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("Letter에서 Player 태그 오브젝트 못 찾음");
            return;
        }
        // 2. 모든 자식 트랜스폼을 돌면서 이름이 playerCandleLightName 인 걸 찾기
        //    (비활성화 포함)
        Transform[] children = player.GetComponentsInChildren<Transform>(true);
        foreach (var t in children)
        {
            if (t.name == playerCandleLightName)
            {
                playerCandleLight = t.gameObject;
                Debug.Log("Letter: 플레이어 양초 불빛 찾음 -> " + t.name);
                break;
            }
        }

        if (playerCandleLight == null)
        {
            Debug.LogWarning($"Letter: 플레이어 자식에서 '{playerCandleLightName}' 이름을 찾지 못함");
        }
    }
}
