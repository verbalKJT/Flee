using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InteractablePiano : MonoBehaviour, IInteractable
{
    public string endingSceneName = "EndingCinematic"; // 엔딩 시네마틱 씬 이름
    public AudioSource pianoMusic; // EndingRoomDoor에서 사용된 AudioSource 공유

    public float fadeDuration = 3f; // 페이드 아웃 시간

    private bool isEndingTriggered = false;

    public void Interact()
    {
        if (isEndingTriggered) return;
        isEndingTriggered = true;

        // 사용자 입력 차단
        DisablePlayerInput();
        
        // 음악 페이드 아웃 후 씬 이동
        StartCoroutine(FadeOutMusicAndLoadScene());
    }

    public string GetPromptText()
    {
        return "[E] 피아노 멈추기";
    }

    private void DisablePlayerInput()
    {
        // 가장 간단하게는 Time.timeScale = 0으로 하겠지만,
        // 아래처럼 커스텀 입력 차단을 사용하는 게 더 안정적
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var controller = player.GetComponent<PlayerMovement>(); // 사용자 스크립트 이름에 맞게 수정
            if (controller != null)
            {
                controller.enabled = false;
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator FadeOutMusicAndLoadScene()
    {
        if (pianoMusic != null)
        {
            float startVolume = pianoMusic.volume;

            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                pianoMusic.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }

            pianoMusic.Stop();
        }

        SceneManager.LoadScene(endingSceneName);
    }
}
