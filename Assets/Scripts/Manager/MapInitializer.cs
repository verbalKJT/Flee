    using System.Collections;
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

        [Header("Starting Room 편지")] 
        [SerializeField] private GameObject LetterCanvas;
        [SerializeField] private Letter letter;
        private GameObject openText;
        private GameObject letterImage; // 편지 UI
        [SerializeField] private Animator doorAnimator; // StartingRoom Animator 문

        [Header("Game Over - Monsters")] [SerializeField]
        private DeadGameover mainMonGameOverScript;
        [SerializeField] private DeadGameover middleMonGameOverScript;
        [SerializeField] private GameObject monsterPanel;
        
        [Header("EndCorridor")] [SerializeField]
        private EndCorridorGameManager endCorridor;
        [SerializeField] private GameObject holywayPrefab;

        [Header("EndingRoom")] [SerializeField]
        private EndingRoomDoor endingRoomDoor;
            
        public void Initialize(GameManager manager)
        {
            Debug.Log("외부 주입 시작");
            endCorridor = GameObject.Find("EndCorriderGameManager").GetComponent<EndCorridorGameManager>();
            
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 오브젝트
            
            LetterCanvas = GameObject.Find("LetterCanvas");
            
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            
            monsterPanel = GameObject.Find("InteractionCanvas").transform.Find("MiddleMonPanel").gameObject;
            if (player != null)
            {
                if (letter != null)
                {
                    StartCoroutine(LetterInjection(player.transform, LetterCanvas));
                }
                ;
                if (mainMonGameOverScript != null && middleMonGameOverScript != null)
                {
                    mainMonGameOverScript.SetPlayerWithUi(player);
                    middleMonGameOverScript.SetPlayerWithUi(player);
                    if (mainCamera != null)
                    {
                        mainMonGameOverScript.SetTrackBinding(mainCamera,monsterPanel); // 타임라인 시네너신 브레인 바인딩
                        middleMonGameOverScript.SetTrackBinding(mainCamera,monsterPanel);
                    }
                }

                if (endingRoomDoor != null)
                    endingRoomDoor.SetupPlayerTransform(player.transform);
                if (endCorridor != null)
                {
                    endCorridor.SetHolyway(holywayPrefab);
                    endCorridor.StartEndCorridor(); // 주입후 시작 
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
        
        private IEnumerator LetterInjection(Transform player, GameObject letterCanvas)
        {
            yield return null;
            if (letterCanvas != null)
            {
                letter.SetPlayerTransform(player.transform);
                openText = LetterCanvas.gameObject.transform.GetChild(1).gameObject;
                letterImage = LetterCanvas.gameObject.transform.GetChild(0).gameObject;
                letter.SetupEnvironment(doorAnimator, openText, letterImage);
            }
        }
    }