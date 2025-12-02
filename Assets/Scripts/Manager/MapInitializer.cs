    using System.Collections;
    //using Microsoft.Unity.VisualStudio.Editor;
    using UnityEngine;

    /// <summary>
    ///  Addressables 로드로 인해 끊어진 외부 참조를 복구
    /// Map 관련 참조들은 반드시 이 클래스를 경유해야함.
    /// </summary>
    public class MapInitializer : MonoBehaviour
    {
        [Header("Main Camera")]
        [SerializeField] private GameObject mainCamera;
        
        [Header("NavMeshMonsters")] [SerializeField]
        private Monster[] navMeshMons; // Main , Middle, sub 순

        [Header("Camera Holder")]
        private GameObject playerCameraHolder;

        [Header("VR Tutorial Canvas")]
        [SerializeField] private GameObject tutorialCanvas;

        [Header("Starting Room 편지")] 
        [SerializeField] private GameObject LetterCanvas;
        [SerializeField] private Letter letter;
        private GameObject openText;
        private GameObject letterImage; // 편지 UI
        [SerializeField] private Animator doorAnimator; // StartingRoom Animator 문

        [Header("Game Over - Monsters")] [SerializeField]
        private DeadGameover mainMonGameOverScript;
        [SerializeField] private DeadGameover middleMonGameOverScript;
        [SerializeField] private DeadGameoverSubMonster SubMonGameOverScript;
        [SerializeField] private GameObject monsterPanel;
        
        [Header("EndCorridor")] [SerializeField]
        private EndCorridorGameManager endCorridor;
        [SerializeField] private GameObject holywayPrefab;

        [Header("EndingRoom")] [SerializeField]
        private EndingRoomDoor endingRoomDoor;

        //Bathroom 천장 트리거
        [Header("CeilingWaterTrigger")][SerializeField]
        private CeilingWaterTrigger ceilingWaterTrigger;

        //WaterManager Water_Cam
        [Header("WaterManager")][SerializeField]
        private WaterManager waterManager;

    //ResetBathroom Water_Cam
    [Header("ResetBathroom")]
    [SerializeField]
    private ResetBathroom resetBathroom;
    
    [Header("맵 속 손전등")]
    [SerializeField] private FlashlightPickup _flashlightPickup;

    [Header("텔레포트 위치")]
    [SerializeField] private Transform teleportPoint;

    [Header ("작업실 맵")]
    [SerializeField] private GameObject map1;
    [SerializeField] private GameObject map2;


    public void Initialize(GameManager manager)
    {
            Debug.Log("외부 주입 시작");
            endCorridor = GameObject.Find("EndCorriderGameManager").GetComponent<EndCorridorGameManager>();
            
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 오브젝트
            
            LetterCanvas = GameObject.Find("LetterCanvas");

            Transform camHolder = player.transform.Find("CameraHolder"); // CameraHolder 찾기
            if (camHolder != null)
            {
                SubMonGameOverScript.SetPlayerCameraHolder(camHolder.gameObject);
            }
        // PC 버전
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        // VR 버전
        mainCamera = GameObject.FindGameObjectWithTag("VRCam");
            tutorialCanvas = GameObject.Find("TutorialCanvas");
            monsterPanel = GameObject.Find("InteractionCanvas").transform.Find("MiddleMonPanel").gameObject;
        
            // 텔레포트 위치 바인딩
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.SetTeleportPoint(teleportPoint);

        if (tutorialCanvas != null && mainCamera != null)
            {
                Canvas canvas = tutorialCanvas.GetComponent<Canvas>();
                if (canvas != null)
                {
                    canvas.worldCamera = mainCamera.GetComponent<Camera>();
                    Debug.Log("📌 TutorialCanvas에 Event Camera 설정 완료");
                }
                else
                {
                    Debug.LogWarning("TutorialCanvas에 Canvas 컴포넌트를 찾을 수 없습니다.");
                }
            }
            else
            {
                Debug.LogWarning("TutorialCanvas 또는 MainCamera가 null입니다.");
            }
        if (player != null)
            {
                if (letter != null)
                {
                    StartCoroutine(LetterInjection(LetterCanvas));
                }
                ;
                if (mainMonGameOverScript != null && middleMonGameOverScript != null && SubMonGameOverScript != null)
                {
                    mainMonGameOverScript.SetPlayerWithUi(player);
                    middleMonGameOverScript.SetPlayerWithUi(player);
                    SubMonGameOverScript.SetPlayerWithUi(player);
                    if (mainCamera != null)
                    {
                        mainMonGameOverScript.SetTrackBinding(mainCamera,monsterPanel); // 타임라인 시네너신 브레인 바인딩
                        middleMonGameOverScript.SetTrackBinding(mainCamera,monsterPanel);
                        SubMonGameOverScript.SetTrackBinding(mainCamera, monsterPanel);
                    }
                }

            if (endingRoomDoor != null)
                    endingRoomDoor.SetupPlayerTransform(player.transform);
                if (endCorridor != null)
                {
                    endCorridor.SetHolyway(holywayPrefab);
                    endCorridor.StartEndCorridor(); // 주입후 시작 
                }

            //마네킹
            InjectMannequins(player);


            //Bathroom 천장 트리거                
            ceilingWaterTrigger.setPlayer(player);

            StartCoroutine(AssignWaterCam(player));

            // 손전등 바인딩
            GameObject flashLight = player.transform.
                Find("OVRCameraRigGame/TrackingSpace/LeftHandAnchor/OVRCustomHandPrefab_L/Flashlight").gameObject;
            GameObject flashlightUI = player.transform.Find("OVRCameraRigGame/TrackingSpace/CenterEyeAnchor" +
                                                            "/CanvasHolder/CrosshairCanvas/FlashLightUI").gameObject;
            // 플레이어 하위 손전등 찾아서 바인딩
            _flashlightPickup.SetFlashlight(flashLight,flashlightUI);
            
            // 책 UI 바인딩 처리
            Transform interactionCanvas = GameObject.Find("InteractionCanvas")?.transform;
                GameObject openBookUI = interactionCanvas?.Find("InteractionOpenBookImg")?.gameObject;

                if (openBookUI == null)
                {
                    Debug.LogWarning("InteractionOpenBookImg를 찾을 수 없습니다.");
                }
                else
                {
                    InteractableBook[] books = FindObjectsOfType<InteractableBook>();

                    foreach (var book in books)
                    {
                        book.hintUI = openBookUI;
                    }
                }
            }
            else
            {
                Debug.Log("Player 못참음");
            }
            
            StartCoroutine(ActiveNavMeshMons(navMeshMons, player.transform));
        }

        private IEnumerator ActiveNavMeshMons(Monster[] navMeshMons, Transform playerTransform)
        {
            yield return null; // 한 프레임 쉬기 
            if (navMeshMons.Length > 0)
            {
                foreach (var mons in navMeshMons)
                {
                    mons.gameObject.SetActive(true);
                    mons.SetupPlayerTransform(playerTransform);
                    mons.OnPlayerSetupComplete();
                }
            }
        }
        
        private IEnumerator LetterInjection(GameObject letterCanvas)
        {
            yield return null;
            if (letterCanvas != null)
            {
                // LetterImage 오브젝트 찾기
                GameObject letterImage = letterCanvas.transform.Find("Letter")?.gameObject;
                if (letterImage == null)
                {
                    Debug.LogWarning("LetterImage를 찾지 못했습니다.");
                    yield break;
                }
                // 편지 객체에 UI와 Animator 연결
                letter.SetupEnvironment(doorAnimator, letterImage);
            }
        }

    void InjectMannequins(GameObject player)
    {
        GameObject[] mannequins = GameObject.FindGameObjectsWithTag("Mannequin");

        foreach (GameObject go in mannequins)
        {
            Mannequin mannequin = go.GetComponent<Mannequin>();
            if (mannequin != null)
            {
                mannequin.SetPlayer(player.transform); 
            }
        }

        Debug.Log($"마네킹 {mannequins.Length}개에 플레이어 주입 완료");
    }

    IEnumerator AssignWaterCam(GameObject player)
    {
        yield return null; // 한 프레임 대기
        GameObject water_Cam = GameObject.FindGameObjectWithTag("Water_Cam");
        Debug.Log(water_Cam.tag);
        if (water_Cam != null)
        {
            waterManager.setCameraOverlay(water_Cam.gameObject);
            resetBathroom.setCameraOverlay(water_Cam.gameObject);
            Debug.Log(" Water_Cam 연결 완료");

            // 연결 후 비활성화
            water_Cam.SetActive(false);
            Debug.Log(" 연결 후 비활성화");
        }
        else
        {
            Debug.LogWarning("Water_Cam 오브젝트를 찾을 수 없습니다.");
        }
    }

    
}