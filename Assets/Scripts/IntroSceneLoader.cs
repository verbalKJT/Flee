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
        // IntroCinematic ���� ��׶��忡�� �̸� �ε�
        asyncLoad = SceneManager.LoadSceneAsync("IntroCinematic");
        asyncLoad.allowSceneActivation = false; // ���� ��ȯ�� ���� ����
        Debug.Log("�� �� ��׶��� �ε� ����");
    }

    public void OnClickStart()
    {
        StartCoroutine(FadeOutAndLoadScene("IntroCinematic"));
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        Debug.Log("�� Fade ����");
        // 1. ���� 0 �� 1 �� ���� ��Ӱ�
        float timer = 0f;
        Color color = fadePanelImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadePanelImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        Debug.Log("�� �� Ȱ��ȭ ����");
        asyncLoad.allowSceneActivation = true; // ���⼭ ������ �� ��ȯ
    }
    public void OnClickSettings()
    {
        Debug.Log("ȯ�漳�� UI ����");
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
