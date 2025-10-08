using System.Collections;
using UnityEngine;
// using UnityEngine.SceneManagement; // �������� �� ��ȯ �� �Ÿ� �ּ� ����

public class EndCorridorGameManager : MonoBehaviour
{
    public static EndCorridorGameManager I;
    void Awake() => I = this;

    [Header("Prefabs & Refs")] 
    private GameObject holywayPrefab;   // ���� ������ (Holyway ������Ʈ ����)
    public Transform spawnRoot;        // ���� ���� ���� ��ġ/ȸ�� (GameManager �ڽ��� �ᵵ OK)
    public Transform player;           // �÷��̾� Transform

    [Header("Progress")]
    public int totalStages = 4;        // �� �������� ���� (1..4)
    private int currentStage = 1;      // ����(������ ������) �������� ��ȣ
    private holyway current;           // ���� ����ִ� ����
    private CharacterController cc;
    private bool inTransition = false; // �ߺ� Ʈ���� ����

    public void StartEndCorridor() // 기존 Start 함수 내용
    {
        cc = player.GetComponent<CharacterController>();

        // ����: ù ���� ���� (ó���� ������ ���� �ʴ� �䱸��� ���� �� �� �� Teleport�� ����)
        if (holywayPrefab != null)
        { ;
            current = SpawnNewRoom(currentStage);
        }
        // ó���� �������� �ʴ´� �� �ּ� ����
        // TeleportTo(current.startPoint.position);
    }
    public void OnEndReached(holyway ended)
    {
        if (inTransition || ended != current) return;
        inTransition = true;

        // ���� ���������� �������� ����
        if (currentStage >= totalStages)
        {
            OnFinishedAll();
            inTransition = false;
            return;
        }

        // ���� ���� ����
        currentStage++;
        var next = SpawnNewRoom(currentStage);

        // �� �䱸����: ���� ���� ����, �� ������ ���� ������ ����
        TeleportTo(next.startPoint.position);

        // ���� ���� ����
        current.OnBeforeDestroyed();
        Destroy(current.gameObject);

        current = next;
        inTransition = false;
    }

    holyway SpawnNewRoom(int stageIndex)
    {
        var go = Instantiate(holywayPrefab, spawnRoot.position, spawnRoot.rotation, spawnRoot);
        var hw = go.GetComponent<holyway>();
        hw.OnSpawned(stageIndex); // �������� ��ȣ ���� �� ���������� ���� ��ġ
        return hw;
    }

    void TeleportTo(Vector3 pos)
    {
        if (cc != null) { cc.enabled = false; player.position = pos; cc.enabled = true; }
        else player.position = pos;
    }

    void OnFinishedAll()
    {
        Debug.Log("[CorridorGameManager] ��� �������� ��� �Ϸ�!");
        // �ʿ� �� �� ��ȯ/������ ��
        // SceneManager.LoadScene("NextSceneName");
    }

    public void SetHolyway(GameObject holyway)
    {
        this.holywayPrefab = holyway;
    }
}
