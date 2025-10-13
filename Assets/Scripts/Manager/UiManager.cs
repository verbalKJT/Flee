using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    [SerializeField] private GameObject settingPanel;
    // 세팅 패널 내 Drowdown 및 Silder
    [SerializeField]private TMP_Dropdown windowDropdown;
    [SerializeField]private TMP_Dropdown resolutionDropdown;
    [SerializeField]private Scrollbar volumeScrollbar;
    [SerializeField]private TMP_Text quitButton;
    public static UiManager instance { get; private set; } // Property를 통해 외부 사용

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

    public void OnSetting()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(!settingPanel.activeSelf);
        }
    }

    public void OffPause()
    {
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.P))
        {
            if (pauseCanvas != null)
            {
                pauseCanvas.SetActive(!pauseCanvas.activeSelf);
                if (pauseCanvas.activeSelf)
                {
                    Time.timeScale = 0;
                    pauseCanvas.GetComponent<Canvas>().sortingOrder = 2;
                }
                else
                {
                    Time.timeScale = 1;
                    pauseCanvas.GetComponent<Canvas>().sortingOrder = 0;
                }
            }
            else if (settingPanel != null)
            {
                settingPanel.SetActive(!settingPanel.activeSelf);
            }
        }
    }
    public void Register(GameObject pause, GameObject setting, TMP_Dropdown windowDropdown, TMP_Dropdown resolutionDropdown, Scrollbar volumeScrollbar)
    {
        pauseCanvas = pause;
        settingPanel = setting;
        this.windowDropdown = windowDropdown;
        this.resolutionDropdown = resolutionDropdown;
        this.volumeScrollbar = volumeScrollbar;

        pauseCanvas.transform.Find("contiuneB").GetComponent<Button>().onClick.AddListener( GameManager.instance.continueGame);
        pauseCanvas.transform.Find("settingB").GetComponent<Button>().onClick.AddListener(OnSetting);
        pauseCanvas.transform.Find("settingB").GetComponent<Button>().onClick.AddListener(OffPause);
        pauseCanvas.transform.Find("exitB").GetComponent<Button>().onClick.AddListener( GameManager.instance.GoToMainMenu);
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
}