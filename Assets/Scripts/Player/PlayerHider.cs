using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PlayerHider : MonoBehaviour
{
    private LibraryHidingQTE qte;
    private HidingZone currentZone;             //플레이어가 숨는 공간
    public TextMeshProUGUI hidePromptText;      //출력할 프롬프트 메세지
    public bool IsHiding { get; private set; }  //플레이어가 숨는 상태를 나타내는 프로퍼티
    private Animator animator;
    public PlayerMovement playerMovement;
    public Transform cameraHolder;              //카메라의 위치
    private Vector3 initialCameraLocalPos;      //처음 카메라의 위치
    private Vector3 originalPosition;           //숨기 전 카메라의 위치 정보 저장을 위한 객체
    private Quaternion originalRotation;        //숨기 전 카메라의 회전
    public List<GameObject> BodyObjects;      //숨을때 숨겨 줄 오브젝트 리스트

    private void Start()
    {
        animator = GetComponent<Animator>();
        qte = FindAnyObjectByType<LibraryHidingQTE>();
        if(cameraHolder != null)
            //처음의 카메라 위치를 저장
            initialCameraLocalPos = cameraHolder.localPosition;
    }
    void Update()
    {
        //숨는 공간 콜리더 안에서 플레이어가 E키를 누르면
        if (currentZone != null && Input.GetKeyDown(KeyCode.E))
        {
            //숨는 중이 아니라면
            if(!IsHiding) 
            {
                PerformHide();
                //QTE가 필요한 공간이라면(서재방)
                if (currentZone.isQTERequired)
                {
                    if (qte != null)
                    {
                        StartCoroutine(DelayedQTEStart(qte));
                    }
                }
                
            }
            //숨는 중이라면
            else
            {
                ExitHide();
            }
        }
    }

    //숨는 공간 콜리더로 플레이어가 들어가면 실행하는 함수
    public void EnterHidingZone(HidingZone zone)
    {
        currentZone = zone;

        if (hidePromptText != null)
        {
            
            //UI 출력
            hidePromptText.gameObject.SetActive(true);
        }
    }
    
    //숨는 공간 콜리더에서 플레이어가 나가면 실행할 함수
    public void ExitHidingZone()
    {
        currentZone = null;

        if (hidePromptText != null)
        {
            hidePromptText.gameObject.SetActive(false);
        }
        IsHiding = false;
    }

    //QTE 결과 처리
    void OnQTEResult(bool success)
    {
        //QTE에 실패하면 강제로 숨기 해제
        if (!success)
        {
            ExitHide();
        }
    }
    //숨기 기능 구현 함수
    void PerformHide()
    {
        if (currentZone != null && currentZone.hidingSpot != null)
        {
            //숨기전 플레이어의 위치 정보를 저장
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            
            //플레이어를 숨는 공간(가구 밑 등)으로 이동시킴
            transform.position = currentZone.hidingSpot.position;
            transform.rotation = currentZone.hidingSpot.rotation;

            if (cameraHolder != null)
            {
                //카메라의 위치를 플레이어 캐릭터의 시점과 맞춰줌
                cameraHolder.localPosition = new Vector3(0, 0.5f, 0.5f);

                //시점이 벽을 보지 않도록 설정
                Vector3 lookDirection = -currentZone.hidingSpot.forward; 
                transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            }
            
            IsHiding = true;
            //나가기 메세지 출력
            if (hidePromptText != null)
            {
                hidePromptText.text = "Press E to exit";
            }
            //숨는 중 플레이어의 움직임 비활성화
            if(playerMovement != null)
            {
                playerMovement.enabled = false;
            }
            //숨는 중 플레이어 시점에서 머리가 보이지 않도록 오브젝트들 비활성화
            foreach (var obj in BodyObjects)
                if (obj != null) obj.SetActive(false);
    

            animator.SetBool("isHiding", true);
            Debug.Log("isHiding true 설정");
            Debug.Log("숨기 실행");
        }
    }
    //숨기 해제 기능 함수
    void ExitHide()
    {
        //플레이어의 움직임을 다시 활성화
        if (playerMovement != null)
            playerMovement.enabled = true;

        //애니메이션 전환을 위해 숨기 조건값 변경
        animator.SetBool("isHiding", false);
        IsHiding = false;

        //QTE가 실행중 숨기해제하면 QTE도 종료
        if (qte != null)
            qte.CancelQTE();

        if (hidePromptText != null)
        {
            hidePromptText.text = "Press E to hide";
        }

        if (cameraHolder != null)
            cameraHolder.localPosition = initialCameraLocalPos;

        Debug.Log("플레이어의 숨기 전 위치: " + originalPosition);

        //플레이어 이동 시 CharacterController와의 충돌 방지를 위해 잠시 비활성화
        CharacterController cc = GetComponent<CharacterController>();
        if(cc != null)
        {
            cc.enabled = false;
            //숨기전의 위치로 플레이어를 이동
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            cc.enabled = true;
        }
        //숨겼던 머리, 머리카락 오브젝트 다시 활성화
        foreach (var obj in BodyObjects)
            if (obj != null) obj.SetActive(false);

        Debug.Log("숨기 해제 실행");
    }
    //QTE를 숨은 후 1초뒤 실행하도록 하는 코루틴 함수
    IEnumerator DelayedQTEStart(LibraryHidingQTE qte)
    {
        yield return new WaitForSeconds(1f);
        qte.BeginQTE(OnQTEResult);
    }
}
