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

    // -------------------- 👹 몬스터 자동 개폐 --------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainMon"))
        {
            monsterInside = true;

            // 이미 코루틴 실행 중이면 중복 방지
            if (doorRoutine != null)
                StopCoroutine(doorRoutine);

            doorRoutine = StartCoroutine(OpenDoorForMonster());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainMon"))
        {
            monsterInside = false;

            // 일정 시간 뒤에 닫기 (나갔다가 다시 감지되는 깜빡임 방지)
            if (doorRoutine != null)
                StopCoroutine(doorRoutine);

            doorRoutine = StartCoroutine(CloseDoorAfterDelay(1.5f));
        }
    }

    private IEnumerator OpenDoorForMonster()
    {
        if (!isOpen)
            OpenDoor();

        // 몬스터가 트리거 안에 있는 동안 문은 계속 열림 상태 유지
        while (monsterInside)
            yield return null;
    }

    private IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 여전히 주변에 몬스터 없을 때만 닫기
        if (!monsterInside)
            CloseDoor();
    }

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

        if (doorOpenSFX != null)
            audioSource.PlayOneShot(doorOpenSFX);
    }

    private void CloseDoor()
    {
        if (isOpen == false) return;

        isOpen = false;

        // 1️⃣ 문 닫기 애니메이션 재생
        animator.SetTrigger("Door1Close");
        animator.SetTrigger("Door2Close");

        if (doorCloseSFX != null)
            audioSource.PlayOneShot(doorCloseSFX);

        // 2️⃣ NavMeshObstacle은 문이 완전히 닫힌 후 활성화
        StartCoroutine(EnableObstacleAfterDelay(1.0f)); // 애니메이션 길이에 맞춰 조절
    }

    private IEnumerator EnableObstacleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (navMeshObstacle != null)
            navMeshObstacle.enabled = true;
    }
    public bool IsOpen() => isOpen;

    public string GetPromptText()
    {
        return isOpen ? "[E] 문 닫기" : "[E] 문 열기";
    }
}
