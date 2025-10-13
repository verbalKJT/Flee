using System.Collections;
using UnityEngine;

public class Letter : MonoBehaviour
{
    private Transform _player; // 플레이어 위치
    /*
    // 💡 PlayerTransform 속성: MapInitializer가 이 속성을 통해 값을 주입
    public Transform PlayerTransform
    {
        set
        {
            _player = value; // 값 할당 (this._player = player; 역할)
            Debug.Log("Player 참조 할당 완료."); // 주입 완료 로그
        }
    } 
    */
    [SerializeField] private Transform letterObject; // 편지 오브젝트 위치 (예: Newspapers_01)
    private GameObject openText;
    private float interactionDistance = 3f; // 상호작용 거리

    private GameObject letterImage; // 편지 UI 
    private Animator doorAnimator; // StartingRoom Animator
    private bool isInRange = false; // 플레이어가 범위 내에 있는지 여부
    private bool isReading = false; // 편지 UI가 열려있는지 여부
    private bool isOpenDoor = false; // 문이 열려있는지
    
    void Update()
    {
        if (_player == null)
        {
            return; // Player가 할당되기 전 Update 한번 종료 
            Debug.Log("Player is null");
        }
        // 플레이어와 편지 오브젝트 거리 계산
        float dist = Vector3.Distance(_player.position, letterObject.position);
        isInRange = dist <= interactionDistance;

        if (isInRange  && !isReading)
        {
            openText.SetActive(true);
        }
        else
        {
            openText.SetActive(false);
        }
        
        // E 키 입력으로 편지 열기
        if (isInRange && !isReading && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(DelayOpenLetter());
        }

        // E 또는 esc 키 입력으로 편지 닫기
        if (isReading && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CloseLetter();
        }
    }

    void OpenLetter()
    {
        letterImage.SetActive(true);
        isReading = true;
        doorAnimator.SetTrigger("Open");
    }

    void CloseLetter()
    {
        letterImage.SetActive(false); // UI 숨김
        isReading = false;
    }

    private IEnumerator DelayOpenLetter()
    {
        yield return null; // 한 프레임 대기
        OpenLetter();
    }
    public void SetupEnvironment(Animator doorAnimator, GameObject openText, GameObject letterImage)
    {
        this.doorAnimator = doorAnimator;
        this.openText = openText;
        this.letterImage = letterImage;
        
        Debug.Log("Letter 환경 설정 완료: Door Animator 및 UI 참조 할당.");
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this._player = playerTransform;
    }
}