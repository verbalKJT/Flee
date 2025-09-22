using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UiMon; // 프리펩 받기

    void Start()
    {
        StartCoroutine(ActiveUiMonster(3f));
    }

    private IEnumerator ActiveUiMonster(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        UiMon.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("IntroCinematic");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
}
