using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;
    public float regenRate = 10f;
    public float drainRate = 25f;

    private bool isExhausted = false;
    public bool isRunning = false;

    //ü�� �������°� �ƴϸ� true�� ��ȯ�ϴ� �ϱ� ���� ������Ƽ
    public bool CanRun => !isExhausted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        //�޸��� �� Ż�����°� �ƴϸ�
        if (isRunning && !isExhausted)
        {
            //���¹̳� ����
            currentStamina -= drainRate * Time.deltaTime;
            //���¹̳��� ����� �ִ� ���̷� ����
            currentStamina = Mathf.Clamp(currentStamina,0, maxStamina);

            if (currentStamina <= 0.01f)
                isExhausted = true;
        }
        //�޸��� ���� �ƴ� ��
        else
        {
            //���¹̳� ȸ��
            currentStamina += regenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            if (isExhausted && currentStamina >= maxStamina)
                isExhausted = false;
        }
    }
}
