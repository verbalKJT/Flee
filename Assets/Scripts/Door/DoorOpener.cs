using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]  // AudioSource 필수 추가
public class DoorOpener : MonoBehaviour, IInteractable
{
    public enum BGMType { None, LivingRoom, Middle, EndingCorridor }

    public BGMType bgmType = BGMType.None;
    public bool shouldChangeBGM = false;  // 이 문이 BGM을 바꿔야 하는가?

    [Header("문 효과음")]
    public AudioClip doorOpenSFX;
    public AudioClip doorCloseSFX;

    private Animator animator;
    private bool isOpen = false;
    private AudioSource audioSource;

    //문 잠김 여부
    public bool isLocked = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            animator.SetTrigger("Door1Open");
            animator.SetTrigger("Door2Open");

            if (doorOpenSFX != null)
                audioSource.PlayOneShot(doorOpenSFX);
        }
        else
        {
            animator.SetTrigger("Door1Close");
            animator.SetTrigger("Door2Close");

            if (doorCloseSFX != null)
                audioSource.PlayOneShot(doorCloseSFX);
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
    //문 잠그기
    public void LockDoor()
    {
        isLocked = true;
        Debug.Log($"{gameObject.name} 문 잠금");
    }
    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log($"{gameObject.name} 문 잠금 해제");
    }

    // ✅ ForceCloseDoor 추가 (다른 부분은 전혀 수정 없음)
    public void ForceCloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            animator.SetTrigger("Door1Close");
            animator.SetTrigger("Door2Close");

            if (doorCloseSFX != null)
                audioSource.PlayOneShot(doorCloseSFX);

            Debug.Log($"🔒 {gameObject.name} 강제로 닫힘 (ForceCloseDoor)");
        }
    }

    // IInteractable 구현
    public void Interact()
    {
        if (isLocked)
        {
            Debug.Log($"{gameObject.name} 은(는) 열리지 않는다.");
            return;
        }

        //StoryDoorLock.cs 유무 검사
        StoryDoorLock storyDoorLock=GetComponent<StoryDoorLock>();
        if(storyDoorLock != null)
        {
            //storyDoorLock.cs가 있다면 스토리용 아이템 개수 검사 후 로직 실행
            Debug.Log("storyDoorLock 있음");
            
            if (storyDoorLock.IsPossibleOpen())
            {//문동작 가능인경우(필요아이템 개수 조건 충족)
                Debug.Log("소지 아이템 개수 충족!");
                ToggleDoor();
            }
            else
            {
                        //아이템 개수 부족! 문을 열 수 없습니다 프롬프트 구현하기
                Debug.Log("소지 아이템 개수 부족!or 필요 동작 미수행");
            }
            
        }
        else
        {
            //storyDoorLock.cs가 안붙어있다면 그냥 일반 문 처럼 작동
            ToggleDoor();
        }

           

        // BGM 변경 로직
        if (shouldChangeBGM && BGMManager.Instance != null)
        {
            switch (bgmType)
            {
                case BGMType.LivingRoom:
                    BGMManager.Instance.PlayBGMPlaylist(BGMManager.Instance.livingRoomBGM);
                    break;
                case BGMType.Middle:
                    BGMManager.Instance.PlayBGMPlaylist(BGMManager.Instance.MiddleBGM);
                    break;
                case BGMType.EndingCorridor:
                    BGMManager.Instance.PlayBGMPlaylist(BGMManager.Instance.EndingCorriderBGM);
                    break;
            }
        }
    }

    public string GetPromptText()
    {
        //문이 닫혀있고 잠겨있을 때 출력할 메세지-> 미스테리 룸
        if(isLocked && !isOpen)
        {
            return "문이 열리지 않습니다";
        }

        return isOpen ? "[E] 문 닫기" : "[E] 문 열기";
    }
}
