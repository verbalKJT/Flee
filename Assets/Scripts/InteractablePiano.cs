using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InteractablePiano : MonoBehaviour
{
    public string endingSceneName = "EndingCinematic"; // EndingCinematic Scene name
    public TextMeshProUGUI interactionTextUI; // interaction E
    private bool isPlayerNear = false;

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            LoadEndingScene();
        }
    }

    private void LoadEndingScene()
    {
        SceneManager.LoadScene(endingSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 피아노에 접근했습니다.");
            isPlayerNear = true;
            if (interactionTextUI != null)
            interactionTextUI.gameObject.SetActive(true); // 텍스트 켜기
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (interactionTextUI != null)
            interactionTextUI.gameObject.SetActive(false); // 텍스트 끄기
        }
    }
}
