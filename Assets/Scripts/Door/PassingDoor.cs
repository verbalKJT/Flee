using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
public class PassingDoor : MonoBehaviour, IInteractable
{
    [Header("문 효과음")]
    public AudioClip doorOpenSFX;
    public AudioClip doorCloseSFX;

    [Header("NavMesh Obstacle (몬스터용)")]
    [SerializeField] private NavMeshObstacle navMeshObstacle;

    private Animator animator;
    private AudioSource audioSource;
    private bool isOpen = false;

    // 🔥 추가: 플레이어가 트리거 안에 있는지 체크
    private bool playerInside = false;

    private bool monsterInside = false;
    private Coroutine doorRoutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (navMeshObstacle == null)
            navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    // -------------------- 👤 플레이어 상호작용 (E키로 열고 닫기) --------------------
    public void Interact()
    {
        ToggleDoor();
    }

    // -------------------- Trigger --------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;

        if (other.CompareTag("MainMon"))
        {
            monsterInside = true;

            if (doorRoutine != null)
                StopCoroutine(doorRoutine);

            doorRoutine = StartCoroutine(OpenDoorForMonster());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;

        if (other.CompareTag("MainMon"))
        {
            monsterInside = false;

            if (doorRoutine != null)
                StopCoroutine(doorRoutine);

            doorRoutine = StartCoroutine(CloseDoorAfterDelay(1.5f));
        }
    }

    // -------------------- 몬스터 자동 개폐 --------------------
    private IEnumerator OpenDoorForMonster()
    {
        if (!isOpen)
            OpenDoor();

        while (monsterInside)
            yield return null;
    }

    private IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!monsterInside)
            CloseDoor();
    }

    // -------------------- Door Methods --------------------
    public void ToggleDoor()
    {
        if (isOpen)
            CloseDoor();
        else
            OpenDoor();
    }

    private void OpenDoor()
    {
        isOpen = true;

        if (navMeshObstacle != null)
            navMeshObstacle.enabled = false;

        animator.SetTrigger("Door1Open");
        animator.SetTrigger("Door2Open");

        // 🔥 플레이어가 있을 때만 사운드 재생
        if (playerInside && doorOpenSFX != null)
            audioSource.PlayOneShot(doorOpenSFX);
    }

    private void CloseDoor()
    {
        if (!isOpen) return;

        isOpen = false;

        animator.SetTrigger("Door1Close");
        animator.SetTrigger("Door2Close");

        // 🔥 플레이어가 있을 때만 사운드 재생
        if (playerInside && doorCloseSFX != null)
            audioSource.PlayOneShot(doorCloseSFX);

        StartCoroutine(EnableObstacleAfterDelay(1.0f)); // 애니메이션 길이에 맞게 조정 가능
    }

    private IEnumerator EnableObstacleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (navMeshObstacle != null)
            navMeshObstacle.enabled = true;
    }

    // -------------------- Helper --------------------
    public bool IsOpen() => isOpen;

    public string GetPromptText()
    {
        return isOpen ? "[E] 문 닫기" : "[E] 문 열기";
    }
}
