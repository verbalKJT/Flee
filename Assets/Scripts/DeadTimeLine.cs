using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class DeadTimeLine : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject gameOverUI;

    void Start()
    {
        timeline.stopped += OnTimelineFinished; // 종료 이벤트 등록
        gameOverUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name + "충돌");
            timeline.Play(); // 이 때 DeathCam이 Timeline에서 Live로 바뀜
        }
    }

    // stopped 이벤트 발생 시
    private void OnTimelineFinished(PlayableDirector director)
    {
        CanvasGroup canvasGroup = gameOverUI.GetComponent<CanvasGroup>();
        StartCoroutine(FadeInUI(canvasGroup, 2f)); // 2초 동안 페이드 인
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
    }
}