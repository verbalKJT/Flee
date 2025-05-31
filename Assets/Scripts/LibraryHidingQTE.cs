using UnityEngine;
using System.Collections;
using TMPro;
public class LibraryHidingQTE : MonoBehaviour
{
    public TextMeshProUGUI promptText; //화면에 띄울 텍스트
    public float timeLimit = 3f;    //제한 시간

    private bool isRunning = false;     //QTE 실행 여부

    //현재 실행 중인 QTE 코루틴 함수를 저장
    private Coroutine currentQTECoroutine = null;  

    //실행 중인 QTE를 저장하는 함수
    public void BeginQTE(System.Action<bool> onResult)
    {
        if (!isRunning)
        {
            currentQTECoroutine = StartCoroutine(StartQTE(onResult));
        }
    }

    //QTE 실행 코루틴함수
    private IEnumerator StartQTE(System.Action<bool> onResult)
    {
        //중복 실행이 되지 않도록 qte 실행 중이면 함수 종료
        if (isRunning) yield break; 
        isRunning = true;

        //qte중 플레이어가 눌러야 될 키 저장
        KeyCode targetKey = GetRandomLetterKey();
        //저장된 키를 UI로 설정
        promptText.text = $"Press <b>{targetKey}</b>!";
        //UI 활성화
        promptText.gameObject.SetActive(true);

        
        float timer = timeLimit;
        bool success = false;

        //제한시간 안에
        while (timer > 0f)
        {
            //플레이어가 설정된 키를 눌렀으면
            if (Input.GetKeyDown(targetKey))
            {
                //성공
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

    //QTE 실행 중 플레이어가 숨기 해제 했을 때 QTE 종료 시키는 함수
    public void CancelQTE()
    {
        if (currentQTECoroutine != null)
        {
            //QTE 실행 코루틴 종료
            StopCoroutine(currentQTECoroutine);

            promptText.gameObject.SetActive(false);

            isRunning = false;

            currentQTECoroutine = null;
        }
    }
    private KeyCode GetRandomLetterKey()
    {
        //A키부터 Z키까지 랜덤으로 설정
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
