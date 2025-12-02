using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AI; // 👈 추가
using Unity.Cinemachine; // 👈 추가

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UiMon; // ui mon
    public static GameManager instance {get; private set;} // 싱글톤
    
    [SerializeField]private string mapAddress; // 에셋 주소
    private GameObject currentMap; // 생성된 맵 저장
    
    [SerializeField] private GameObject OVRCameraRig; // ProtoUI 씬의 OVRCameraRig 참조
    
    private AsyncOperation asyncLoad;
    
    private AsyncOperationHandle<GameObject> mapHandle;
    
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
        HeadTrackingLock ht = FindObjectOfType<HeadTrackingLock>();
        ht.LockTracking(9.5f);
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
        // SceneManager.LoadScene("IntroCinematic");
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
        var op = Addressables.LoadAssetAsync<GameObject>(mapAddress);
        op.Completed += OnMapLoadCompleted;
        mapHandle = op;
    }
    private void DestroyMap()
    {
        if (currentMap != null)
        {
            Destroy(currentMap);
            currentMap = null;
        }

        if (mapHandle.IsValid())
        {
            Addressables.Release(mapHandle);
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
    // 몬스터 오브젝트가 비활성화되어도 안전하게 리스폰 프로세스를 시작합니다.
    public void StartRespawnRoutine(Transform respawnPos, GameObject player, GameObject mainCam, GameObject monsterUI, NavMeshAgent agent, CapsuleCollider collider)
    {
        // GameManager 오브젝트는 씬이 넘어가도 DestroyOnLoad로 유지되므로 안전합니다.
        StartCoroutine(CallReSpawn(respawnPos, player, mainCam, monsterUI, agent, collider));
    }
    private IEnumerator CallReSpawn(Transform respawnPos, GameObject player, GameObject mainCam, GameObject monsterUI, NavMeshAgent agent, CapsuleCollider collider)
    {
        Debug.Log("GameManager에서 리스폰 코루틴 시작.");
        yield return new WaitForSeconds(0.2f); // 타임라인 종료 대기

        if (respawnPos != null && player != null)
        {
            // 1. Cinemachine Brain 참조 및 원본 Blend Time 저장
            CinemachineBrain brain = mainCam.GetComponent<CinemachineBrain>();
            float originalBlendTime = 0f;
    
            if (brain != null)
            {
                originalBlendTime = brain.DefaultBlend.Time;
                brain.DefaultBlend.Time = 0f; // 즉시 전환 강제
            }
        
            if (monsterUI != null) monsterUI.SetActive(false); // 패널 비활성화
        
            yield return new WaitForSeconds(0.1f); 

            // 몬스터 상태 리셋
            if (collider != null) collider.enabled = true; // 콜라이더 활성화
            if (agent != null) agent.isStopped = false; // 이동 재개

            // 플레이어 리스폰 (GameManager에 이미 PlayerReSpawn 함수가 있음)
            PlayerReSpawn(respawnPos, player); 
            player.SetActive(true);
        
            // ✅ 가장 중요: 몬스터 상태 (hasPlayed) 리셋
            // DeadGameover.cs에서 hasPlayed를 public static으로 변경했으므로 직접 접근 가능합니다.
            DeadGameover.hasPlayed = true; 
        
            // 카메라 블렌드 시간 복구
            if (brain != null)
            {
                yield return null; 
                brain.DefaultBlend.Time = originalBlendTime;
            }
        
            Debug.Log("리스폰 완료 및 몬스터 상태 리셋.");
        }
    }

    public void PlayerReSpawn(Transform spawnPoint,GameObject player)
    {
        Debug.Log("Player respawn");
        player.transform.position = spawnPoint.position; // 플레이어 위치 리스폰 위치로
    }
}