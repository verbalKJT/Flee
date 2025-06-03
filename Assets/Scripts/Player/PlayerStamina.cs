using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;
    public float regenRate = 10f;
    public float drainRate = 25f;

    private bool isExhausted = false;
    public bool isRunning = false;

    //체력 소진상태가 아니면 true를 반환하는 일기 전용 프로퍼티
    public bool CanRun => !isExhausted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        //달리는 중 탈진상태가 아니면
        if (isRunning && !isExhausted)
        {
            //스태미나 소진
            currentStamina -= drainRate * Time.deltaTime;
            //스태미나를 현재와 최대 사이로 제한
            currentStamina = Mathf.Clamp(currentStamina,0, maxStamina);

            if (currentStamina <= 0.01f)
                isExhausted = true;
        }
        //달리는 중이 아닐 때
        else
        {
            //스태미나 회복
            currentStamina += regenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            if (isExhausted && currentStamina >= maxStamina)
                isExhausted = false;
        }
    }
}
