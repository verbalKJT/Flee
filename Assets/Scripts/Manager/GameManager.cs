using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UiMon; // ui mon

    [SerializeField]private GameObject settingPanel; // 세팅 패널

    // 세팅 패널 내 Drowdown 및 Silder
    [SerializeField]private TMP_Dropdown windowDropdown;
    [SerializeField]private TMP_Dropdown resolutionDropdown;
    [SerializeField]private Scrollbar volumeScrollbar;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 모든 씬에서 사용
    }

    void Start()
    {
        StartCoroutine(ActiveUiMonster(3f));
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("1stFloor")) // Main 씬에서 SettingPanel 끄기
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !settingPanel.activeSelf)
            {
                settingPanel.SetActive(true);
                Time.timeScale = 0; // 일시 정지
            }
        }
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
        SceneManager.LoadScene("IntroCinematic");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ApplySettings()
    {
        // 창,전체
        FullScreenMode mode = FullScreenMode.FullScreenWindow;
        switch (windowDropdown.value)
        {
            case 0: mode = FullScreenMode.ExclusiveFullScreen; break;
            case 1: mode = FullScreenMode.FullScreenWindow; break;
        }
        // 해상도 
        string resolution = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] resolutonPart = resolution.Split('x');
        int width = int.Parse(resolutonPart[0]);
        int height = int.Parse(resolutonPart[1]);
        Screen.SetResolution(width, height, mode);

        // 볼륨 설정
        float volume = volumeScrollbar.value;
        AudioListener.volume = volume;
        
        Debug.Log(mode + " " + width + " " + height + " " + volume);
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
        if (scene.name.Equals("1stFloor")) // 인트로랑 메인 씬에서만 찾도록
        {
            settingPanel = GameObject.Find("SettingPanel");
            if (settingPanel != null)
            {
                windowDropdown = settingPanel.transform.Find("windowDropdown").GetComponent<TMP_Dropdown>();
                resolutionDropdown = settingPanel.transform.Find("resolutionDropdown").GetComponent<TMP_Dropdown>();
                volumeScrollbar = settingPanel.transform.Find("volumeScrollbar").GetComponent<Scrollbar>();
            }
        }
        else
        {
            settingPanel = null;
        }

        if (!scene.name.Equals("ProtoUI"))
        {
            UiMon =  null;
        }
    }
}