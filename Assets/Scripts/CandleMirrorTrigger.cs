using UnityEngine;
using TMPro;

public class CandleMirrorTrigger : MonoBehaviour
{
    [Header("촛불 오브젝트")]
    public GameObject[] candleObjects;              // 시선 체크할 촛불 오브젝트
    public GameObject[] candleFlames;                // 여러 개의 촛불 

    [Header("거울 오브젝트")]
    public GameObject[] mirrorObjects;              // Mirror1, Mirror2
    public TextMeshProUGUI interactionText;          // [E] 텍스트
    public Camera playerCamera;                      // 플레이어 카메라

    [Header("상호작용 가능 범위")]
    public float interactDistance = 2f;
    public LayerMask interactLayer;

    private bool[] flameStates;                     // 각 촛불 켜졌는지 체크
    private float messageTimer = 1f;                // 경고 메세지 출력 시간   
    void Start()
    {
        flameStates = new bool[candleFlames.Length];

        // 처음 촛불 Flame 비활성화  
        for (int i = 0; i < candleFlames.Length; i++)
        {
            if (candleFlames[i] != null)
                candleFlames[i].SetActive(false);

            flameStates[i] = false;
        }

        // 힌트 거울 오브젝트 비활성화
        for (int i = 0; i < mirrorObjects.Length; i++)
        {
            if (mirrorObjects[i] != null)
                mirrorObjects[i].SetActive(false);
        }

        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
         // 경고 메시지 타이머 처리
        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f && interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            // 촛불 오브젝트 배열 중 감지된 대상이 있는지 확인
            for (int i = 0; i < candleObjects.Length; i++)
            {
                if ((hit.collider.gameObject == candleObjects[i] || hit.collider.transform.IsChildOf(candleObjects[i].transform)) && !flameStates[i])
                {
                    if (interactionText != null)
                    {
                        interactionText.gameObject.SetActive(true);
                        interactionText.text = "[E] 촛불 키기";
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (FindLighterInBox.hasLighter)
                        {
                            // 불꽃 켜기
                            if (candleFlames[i] != null)
                                candleFlames[i].SetActive(true);

                            flameStates[i] = true;

                            // 텍스트 끄기
                            if (interactionText != null)
                                interactionText.gameObject.SetActive(false);

                            // 모든 촛불이 켜졌는지 확인
                            if (AllCandlesLit())
                            {
                                for (int j = 0; j < mirrorObjects.Length; j++)
                                {
                                    if (mirrorObjects[j] != null)
                                        mirrorObjects[j].SetActive(true);
                                }
                            }
                        }
                        else
                        {
                            // 라이터 없을 때 경고 출력
                            if (interactionText != null)
                            {
                                interactionText.text = "라이터가 없어 촛불을 켤 수 없습니다.";
                                interactionText.gameObject.SetActive(true);
                            }
                            messageTimer = 1f; // 1초 동안 표시
                        }
                    }
                    return;
                }
            }
        }

        // 감지 안되면 텍스트 숨김 (단, 메시지 출력 중이 아닐 때만)
        if (interactionText != null && messageTimer <= 0f)
            interactionText.gameObject.SetActive(false);
    }
    private bool AllCandlesLit()
    {
        foreach (bool lit in flameStates)
        {
            if (!lit) return false;
        }
        return true;
    }
}
