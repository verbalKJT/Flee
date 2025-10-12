using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class DeadGameover : MonoBehaviour, IDeadMon
{
    [Header("Timeline & UI")]
    [SerializeField] private PlayableDirector timeline;
    private GameObject gameOverUI;

    [Header("Player Control")]
    private GameObject playerObject; // Player 오브젝트
    private MonoBehaviour playerControllerScript; // PlayerMovement

    private bool hasPlayed = false;
    
    [Header("Player Respwan 위치")]
    [SerializeField] private Transform playerRespwan;
    
    void Start()
    {
        if (timeline != null)
            timeline.stopped += OnTimelineFinished;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            hasPlayed = true;

            // 플레이어 조작 끄기
            if (playerControllerScript != null)
                playerControllerScript.enabled = false;

            // Timeline 실행
            if (timeline != null)
                timeline.Play();
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        if (gameOverUI == null) return;

        CanvasGroup canvasGroup = gameOverUI.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            StartCoroutine(FadeInUI(canvasGroup, 2f)); // 2초 동안 페이드 인
        }
        else
        {
            GameManager.instance.PlayerReSpawn(playerRespwan,playerObject);
            gameOverUI.SetActive(true); 
            Destroy(gameObject); // fallback
        }
    }

    private IEnumerator FadeInUI(CanvasGroup canvasGroup, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        
        GameManager.instance.PlayerReSpawn(playerRespwan,playerObject); // 플레이어 리스폰
        playerControllerScript.enabled = true; // 리스폰 후 플레이어 이동 활성화
        gameOverUI.SetActive(false); // 게임오버 UI 비활성화 <- TimeLine으로 관리할거면 없애도 됨.
        // MiddleMon 오브젝트 삭제
        // Destroy(gameObject); 
    }
    public void SetPlayerWithUi(GameObject playerObject,  MonoBehaviour playerControllerScript, GameObject gameOverUI)
    {
        this.playerObject = playerObject;
        this.playerControllerScript = playerControllerScript;
        this.gameOverUI = gameOverUI;
    }
}