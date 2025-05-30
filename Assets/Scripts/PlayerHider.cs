using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PlayerHider : MonoBehaviour
{
    private LibraryHidingQTE qte;
    private HidingZone currentZone;             //숨을 공간
    public TextMeshProUGUI hidePromptText;      //Press E to hide를 출력할 UI
    public bool IsHiding { get; private set; }
    private Animator animator;
    public PlayerMovement playerMovement;
    public Transform cameraHolder;              //카메라 위치
    private Vector3 initialCameraLocalPos;      //처음 카메라의 위치
    private Vector3 originalPosition;           //숨기 전 플레이어의 위치
    private Quaternion originalRotation;        //숨기 전 플레이어의 방향
    public List<GameObject> BodyObjects;      //머리 및 머리카락 오브젝트 저장

    private void Start()
    {
        animator = GetComponent<Animator>();
        qte = FindAnyObjectByType<LibraryHidingQTE>();
        if(cameraHolder != null)
            //카메라의 현재 위치를 저장
            initialCameraLocalPos = cameraHolder.localPosition;
    }
    void Update()
    {
        if (currentZone != null && Input.GetKeyDown(KeyCode.E))
        {
            if(!IsHiding)
            {
                PerformHide();

                if (currentZone.isQTERequired)
                {
                    
                    if (qte != null)
                    {
                        StartCoroutine(DelayedQTEStart(qte));
                    }
                }
                
            }
            else
            {
                ExitHide();
            }
        }
    }

    //숨을 공간 근처로 다가갈때
    public void EnterHidingZone(HidingZone zone)
    {
        currentZone = zone;

        if (hidePromptText != null)
        {
            
            //UI 출력
            hidePromptText.gameObject.SetActive(true);
        }
    }
    
    //숨을 공간 근처에서 벗어날 때
    public void ExitHidingZone()
    {
        currentZone = null;

        if (hidePromptText != null)
        {
            hidePromptText.gameObject.SetActive(false);
        }
        IsHiding = false;
    }

    //QTE 결과 처리 함수
    void OnQTEResult(bool success)
    {
        //QTE 실패 시 숨기 강제 해제
        if (!success)
        {
            Debug.Log("QTE실패");
            ExitHide();
        }
    }
    //숨기 함수
    void PerformHide()
    {
        if (currentZone != null && currentZone.hidingSpot != null)
        {
            //숨기 전 플레이어 위치 저장
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            Debug.Log("숨기 전 위치 저장됨: " + originalPosition);

            // 플레이어 위치를 숨는 위치로 이동시킴
            transform.position = currentZone.hidingSpot.position;
            transform.rotation = currentZone.hidingSpot.rotation;

            if (cameraHolder != null)
            {
                //카메라 위치를 캐릭터 시야와 맞춤
                cameraHolder.localPosition = new Vector3(0, 0.5f, 0.5f);

                //숨을 때 플레이어가 벽 반대를 바라보도록 설정
                Vector3 lookDirection = -currentZone.hidingSpot.forward; // 벽 반대 방향
                transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            }

            IsHiding = true;

            if (hidePromptText != null)
            {
                hidePromptText.text = "Press E to exit";
            }
            if(playerMovement != null)
            {
                playerMovement.enabled = false;
            }
            //숨는 동안 머리가 보이지 않도록 비활성화
            foreach (var obj in BodyObjects)
                if (obj != null) obj.SetActive(false);
    

            animator.SetBool("isHiding", true);
            Debug.Log("isHiding true 됨");
            Debug.Log("숨기 성공");
        }
    }
    //숨기 해제 함수
    void ExitHide()
    {
        
        if (playerMovement != null)
            playerMovement.enabled = true;

        animator.SetBool("isHiding", false);
        IsHiding = false;

        if (hidePromptText != null)
        {
            hidePromptText.text = "Press E to hide";
        }

        if (cameraHolder != null)
            cameraHolder.localPosition = initialCameraLocalPos;

        Debug.Log("숨기 해제 위치 복구: " + originalPosition);

        //캐릭터 컨트롤러와 충돌 방지를 위해 비활성화 후 캐릭터 이동
        CharacterController cc = GetComponent<CharacterController>();
        if(cc != null)
        {
            cc.enabled = false;
            //플레이어 캐릭터를 숨기 전 원래 위치로 이동
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            cc.enabled = true;
        }
        //숨기 해제 후 머리가 다시 보이도록 활성화
        foreach (var obj in BodyObjects)
            if (obj != null) obj.SetActive(false);

        Debug.Log("숨기 해제됨");
    }
    //QTE를 숨은 뒤 1초뒤 실행시키는 코루틴 함수
    IEnumerator DelayedQTEStart(LibraryHidingQTE qte)
    {
        yield return new WaitForSeconds(1f);
        yield return qte.StartQTE(OnQTEResult);
    }
}
