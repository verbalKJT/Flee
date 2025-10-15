using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logoText;
    [SerializeField] private TextMeshProUGUI anyKeyText;

    private bool isInputEnabled = false;
    private void Start()
    {
        // 초기엔 안보이게
        logoText.alpha = 0f;
        anyKeyText.alpha = 0f;
    }

    public void PlayEndingUI()
    {
        StartCoroutine(FadeInTMPText(logoText, 2f));
        StartCoroutine(FadeInTMPText(anyKeyText, 3f));
        
        // 1초 뒤부터 입력 받기 (실수 방지용)
        StartCoroutine(EnableInputAfterDelay(1f));
    }

    private IEnumerator FadeInTMPText(TextMeshProUGUI text, float duration)
    {
        float time = 0f;
        float startAlpha = text.alpha;
        float targetAlpha = 0.8f;

        while (time < duration)
        {
            time += Time.deltaTime;
            text.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        text.alpha = targetAlpha;
    }
    private IEnumerator EnableInputAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isInputEnabled = true;
    }

    private void Update()
    {
        if (!isInputEnabled) return;

        if (Input.anyKeyDown)
        {
            // 아무 키 누르면 ProtoUI 씬으로 전환
            SceneManager.LoadScene("ProtoUI");
        }
    }
}
