using System.Collections;
using UnityEngine;

/// <summary>
///  Addressables 로드로 인해 끊어진 외부 참조를 복구
/// Map 관련 참조들은 반드시 이 클래스를 경유해야함.
/// </summary>
public class MapInitializer : MonoBehaviour
{
    [Header("NavMeshMonsters")] [SerializeField]
    private Monster[] navMeshMons;

    [Header("Starting Room 편지")] [SerializeField]
    private Letter letter;

    [SerializeField] private GameObject openText;
    [SerializeField] private GameObject letterImage; // 편지 UI
    [SerializeField] private Animator doorAnimator; // StartingRoom Animator 문

    [Header("Game Over - Monsters")] [SerializeField]
    private DeadGameover mainMonGameOverScript;

    [SerializeField] private DeadGameover middleMonGameOverScript;
    [SerializeField] private GameObject gameOverUI; // gameOver UI

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

        if (player != null)
        {
            if (letter != null)
            {
                letter.PlayerTransform = player.transform;
                letter.SetupEnvironment(doorAnimator, openText, letterImage);
            }

            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (mainMonGameOverScript != null && middleMonGameOverScript != null)
            {
                mainMonGameOverScript.SetPlayerWithUi(player, playerMovement, gameOverUI);
                middleMonGameOverScript.SetPlayerWithUi(player, playerMovement, gameOverUI);
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
}