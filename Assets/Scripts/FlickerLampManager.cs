using UnityEngine;
using System.Collections;

public class FlickerLampManager : MonoBehaviour
{
    [Header ("���� ������Ʈ")]
    public GameObject lampOnObject;
    public GameObject lampOffObject;

    [Header ("������ �ֱ�")]
    public float minInterval = 0.1f;
    public float maxInterval = 0.6f;

    private Coroutine flickerRoutine;

    public void StartFlicker()
    {
        if (flickerRoutine == null)
        {
            flickerRoutine = StartCoroutine(FlickerLoop());
        }
    }

    public void StopFlicker()
    {
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
        }

        // ���� ���� (�׻� Off ���·�)
        if (lampOnObject != null) lampOnObject.SetActive(false);
        if (lampOffObject != null) lampOffObject.SetActive(true);
    }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            // ���
            if (lampOnObject != null && lampOffObject != null)
            {
                bool currentlyOn = lampOnObject.activeSelf;
                lampOnObject.SetActive(!currentlyOn);
                lampOffObject.SetActive(currentlyOn);
            }

            float wait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);
        }
    }
}
