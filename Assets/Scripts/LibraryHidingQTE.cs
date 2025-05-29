using UnityEngine;
using System.Collections;
using TMPro;
public class LibraryHidingQTE : MonoBehaviour
{
    public TextMeshProUGUI promptText; //화면에 띄울 텍스트
    public float timeLimit = 3f;

    private bool isRunning = false;

    public IEnumerator StartQTE(System.Action<bool> onResult)
    {
        if (isRunning) yield break;
        isRunning = true;

        KeyCode targetKey = GetRandomLetterKey();
        promptText.text = $"Press <b>{targetKey}</b>!";
        promptText.gameObject.SetActive(true);

        float timer = timeLimit;
        bool success = false;

        while (timer > 0f)
        {
            if (Input.GetKeyDown(targetKey))
            {
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
