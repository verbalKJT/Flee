using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PlayerHider : MonoBehaviour
{
    private LibraryHidingQTE qte;
    private HidingZone currentZone;             //���� ����
    public TextMeshProUGUI hidePromptText;      //Press E to hide�� ����� UI
    public bool IsHiding { get; private set; }
    private Animator animator;
    public PlayerMovement playerMovement;
    public Transform cameraHolder;              //ī�޶� ��ġ
    private Vector3 initialCameraLocalPos;      //ó�� ī�޶��� ��ġ
    private Vector3 originalPosition;           //���� �� �÷��̾��� ��ġ
    private Quaternion originalRotation;        //���� �� �÷��̾��� ����
    public List<GameObject> BodyObjects;      //�Ӹ� �� �Ӹ�ī�� ������Ʈ ����

    private void Start()
    {
        animator = GetComponent<Animator>();
        qte = FindAnyObjectByType<LibraryHidingQTE>();
        if(cameraHolder != null)
            //ī�޶��� ���� ��ġ�� ����
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

    //���� ���� ��ó�� �ٰ�����
    public void EnterHidingZone(HidingZone zone)
    {
        currentZone = zone;

        if (hidePromptText != null)
        {
            
            //UI ���
            hidePromptText.gameObject.SetActive(true);
        }
    }
    
    //���� ���� ��ó���� ��� ��
    public void ExitHidingZone()
    {
        currentZone = null;

        if (hidePromptText != null)
        {
            hidePromptText.gameObject.SetActive(false);
        }
        IsHiding = false;
    }

    //QTE ��� ó�� �Լ�
    void OnQTEResult(bool success)
    {
        //QTE ���� �� ���� ���� ����
        if (!success)
        {
            ExitHide();
        }
    }
    //���� �Լ�
    void PerformHide()
    {
        if (currentZone != null && currentZone.hidingSpot != null)
        {
            //���� �� �÷��̾� ��ġ ����
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            
            // �÷��̾� ��ġ�� ���� ��ġ�� �̵���Ŵ
            transform.position = currentZone.hidingSpot.position;
            transform.rotation = currentZone.hidingSpot.rotation;

            if (cameraHolder != null)
            {
                //ī�޶� ��ġ�� ĳ���� �þ߿� ����
                cameraHolder.localPosition = new Vector3(0, 0.5f, 0.5f);

                //���� �� �÷��̾ �� �ݴ븦 �ٶ󺸵��� ����
                Vector3 lookDirection = -currentZone.hidingSpot.forward; // �� �ݴ� ����
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
            //���� ���� �Ӹ��� ������ �ʵ��� ��Ȱ��ȭ
            foreach (var obj in BodyObjects)
                if (obj != null) obj.SetActive(false);
    

            animator.SetBool("isHiding", true);
            Debug.Log("isHiding true ��");
            Debug.Log("���� ����");
        }
    }
    //���� ���� �Լ�
    void ExitHide()
    {
        
        if (playerMovement != null)
            playerMovement.enabled = true;

        animator.SetBool("isHiding", false);
        IsHiding = false;

        if (qte != null)
            qte.CancelQTE();

        if (hidePromptText != null)
        {
            hidePromptText.text = "Press E to hide";
        }

        if (cameraHolder != null)
            cameraHolder.localPosition = initialCameraLocalPos;

        Debug.Log("���� ���� ��ġ ����: " + originalPosition);

        //ĳ���� ��Ʈ�ѷ��� �浹 ������ ���� ��Ȱ��ȭ �� ĳ���� �̵�
        CharacterController cc = GetComponent<CharacterController>();
        if(cc != null)
        {
            cc.enabled = false;
            //�÷��̾� ĳ���͸� ���� �� ���� ��ġ�� �̵�
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            cc.enabled = true;
        }
        //���� ���� �� �Ӹ��� �ٽ� ���̵��� Ȱ��ȭ
        foreach (var obj in BodyObjects)
            if (obj != null) obj.SetActive(false);

        Debug.Log("���� ������");
    }
    //QTE�� ���� �� 1�ʵ� �����Ű�� �ڷ�ƾ �Լ�
    IEnumerator DelayedQTEStart(LibraryHidingQTE qte)
    {
        yield return new WaitForSeconds(1f);
        qte.BeginQTE(OnQTEResult);
    }
}
