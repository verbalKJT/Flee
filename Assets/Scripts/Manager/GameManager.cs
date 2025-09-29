using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UiMon; // ui mon
    public static GameManager instance {get; private set;} // 싱글톤
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 중복시 파괴
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // 씬 넘어가도 삭제 안되도록
    }

    void Start()
    {
        StartCoroutine(ActiveUiMonster(3f));
    }

    private IEnumerator ActiveUiMonster(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        if (UiMon != null)
        {
            // 게임 실행 후 spawnTime 안에 게임이 시작될 경우 NullReference 방지
            UiMon.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("1stFloor");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ApplySettings()
    {
        UiManager.instance.ApplySettings();
    }
    // 씬이 넘아가도 넘어간 씬의 SettingPanel을 찾을 수 있게
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("ProtoUI"))
        {
            UiMon =  null;
        }
    }

    public void continueGame()
    {
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("ProtoUI");
    }
}