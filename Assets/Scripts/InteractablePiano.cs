using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractablePiano : MonoBehaviour, IInteractable
{
    public string endingSceneName = "EndingCinematic"; // 엔딩 시네마틱 씬 이름 
    public void Interact()
    {
        SceneManager.LoadScene(endingSceneName);
    }
    public string GetPromptText()
    {
        return "[E] 피아노 연주하기";
    }
}
