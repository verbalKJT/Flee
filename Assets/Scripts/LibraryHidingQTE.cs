using UnityEngine;
using System.Collections;
using TMPro;
public class LibraryHidingQTE : MonoBehaviour
{
    public TextMeshProUGUI promptText; //ȭ�鿡 ��� �ؽ�Ʈ
    public float timeLimit = 3f;    //���� �ð�

    private bool isRunning = false;     //QTE ���� ����

    //���� ���� ���� QTE �ڷ�ƾ �Լ��� ����
    private Coroutine currentQTECoroutine = null;  

    //���� ���� QTE�� �����ϴ� �Լ�
    public void BeginQTE(System.Action<bool> onResult)
    {
        if (!isRunning)
        {
            currentQTECoroutine = StartCoroutine(StartQTE(onResult));
        }
    }

    //QTE ���� �ڷ�ƾ�Լ�
    private IEnumerator StartQTE(System.Action<bool> onResult)
    {
        //�ߺ� ������ ���� �ʵ��� qte ���� ���̸� �Լ� ����
        if (isRunning) yield break; 
        isRunning = true;

        //qte�� �÷��̾ ������ �� Ű ����
        KeyCode targetKey = GetRandomLetterKey();
        //����� Ű�� UI�� ����
        promptText.text = $"Press <b>{targetKey}</b>!";
        //UI Ȱ��ȭ
        promptText.gameObject.SetActive(true);

        
        float timer = timeLimit;
        bool success = false;

        //���ѽð� �ȿ�
        while (timer > 0f)
        {
            //�÷��̾ ������ Ű�� ��������
            if (Input.GetKeyDown(targetKey))
            {
                //����
                success = true;
                break;
            }

            timer -= Time.deltaTime;
            yield return null;
        }

        promptText.gameObject.SetActive(false);
        onResult?.Invoke(success);
        isRunning = false;
    }

    //QTE ���� �� �÷��̾ ���� ���� ���� �� QTE ���� ��Ű�� �Լ�
    public void CancelQTE()
    {
        if (currentQTECoroutine != null)
        {
            //QTE ���� �ڷ�ƾ ����
            StopCoroutine(currentQTECoroutine);

            promptText.gameObject.SetActive(false);

            isRunning = false;

            currentQTECoroutine = null;
        }
    }
    private KeyCode GetRandomLetterKey()
    {
        //AŰ���� ZŰ���� �������� ����
        int ascii = Random.Range((int)KeyCode.A, (int)KeyCode.Z + 1);   
        return (KeyCode)ascii;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
