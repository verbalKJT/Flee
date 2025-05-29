using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class IntroSceneLoader : MonoBehaviour
{
    public Image fadePanelImage;
    public float fadeDuration = 1.0f;

    private AsyncOperation asyncLoad;

    void Start()
    {
        // IntroCinematic 씬을 백그라운드에서 미리 로딩
        asyncLoad = SceneManager.LoadSceneAsync("IntroCinematic");
        asyncLoad.allowSceneActivation = false; // 아직 전환은 하지 않음
        Debug.Log("▶ 씬 백그라운드 로딩 시작");
    }

    public void OnClickStart()
    {
        StartCoroutine(FadeOutAndLoadScene("IntroCinematic"));
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        Debug.Log("▶ Fade 시작");
        // 1. 알파 0 → 1 로 점점 어둡게
        float timer = 0f;
        Color color = fadePanelImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadePanelImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        Debug.Log("▶ 씬 활성화 시작");
        asyncLoad.allowSceneActivation = true; // 여기서 실제로 씬 전환
    }
    public void OnClickSettings()
    {
        Debug.Log("환경설정 UI 열기");
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
