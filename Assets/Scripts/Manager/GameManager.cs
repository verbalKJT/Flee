using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UiMon; // ui mon
    public static GameManager instance {get; private set;} // 싱글톤
    
    [SerializeField]private string mapAddress; // 에셋 주소
    private GameObject currentMap; // 생성된 맵 저장
    
    private AsyncOperation asyncLoad;
    
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
        // IntroCinematic 씬을 백그라운드에서 미리 로딩
        asyncLoad = SceneManager.LoadSceneAsync("IntroCinematic");
        asyncLoad.allowSceneActivation = false; // 아직 전환은 하지 않음
        Debug.Log("▶ 씬 백그라운드 로딩 시작");
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
        asyncLoad.allowSceneActivation = true;
        SceneManager.LoadScene("IntroCinematic");
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

        if (scene.name.Equals("1stFloor"))
        {
            OnLoadMap();
        }
        else
        {
            DestroyMap();
        } 
    }
    
    public void continueGame()
    {
        Time.timeScale = 1;
        UiManager.instance.pauseCanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("ProtoUI");
    }

    private void OnLoadMap()
    {
        if (currentMap != null)
        {
            Destroy(currentMap);
        }
        // 비동기 로드 시작
        Addressables.LoadAssetAsync<GameObject>(mapAddress).Completed += OnMapLoadCompleted;
    }
    private void DestroyMap()
    {
        if (currentMap != null)
        {
            Destroy(currentMap);
            currentMap = null;
        }
    }

    private void OnMapLoadCompleted(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // 로드 성공
            GameObject map = handle.Result; // 로드된 프리팹 인스턴스화
            currentMap = Instantiate(map,Vector3.zero,Quaternion.identity);
            
            MapInitializer initializer = currentMap.GetComponent<MapInitializer>();

            if (initializer != null)
            {
                initializer.Initialize(this);
                Debug.Log("Map initializer call.");
            }
            Debug.Log("Map loaded");
        }
        else
        {
            Debug.Log("Failed to load map");
        }
    }

    public void PlayerReSpawn(Transform spawnPoint,GameObject player)
    {
        Debug.Log("Player respawn");
        player.transform.position = spawnPoint.position; // 플레이어 위치 리스폰 위치로
    }
}