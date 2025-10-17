using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorOpener : MonoBehaviour, IInteractable
{
    public enum BGMType { None, Start, LivingRoom, Middle, EndingCorridor }

    public BGMType bgmType = BGMType.None;
    public bool shouldChangeBGM = false;  // 이 문이 BGM을 바꿔야 하는가?
    
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            animator.SetTrigger("Door1Open");
            animator.SetTrigger("Door2Open");
        }
        else
        {
            animator.SetTrigger("Door1Close");
            animator.SetTrigger("Door2Close");
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
    // IInteractable 구현
    public void Interact()
    {
        ToggleDoor();
        
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

        isOpen = true;
    }
    public string GetPromptText()
    {
        return isOpen ? "[E] 문 닫기" : "[E] 문 열기";
    }
}
