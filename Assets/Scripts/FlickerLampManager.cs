using UnityEngine;
using System.Collections;

public class FlickerLampManager : MonoBehaviour
{
    [Header ("조명 오브젝트")]
    public GameObject lampOnObject;
    public GameObject lampOffObject;

    [Header ("깜빡임 주기")]
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

        // 상태 복원 (항상 Off 상태로)
        if (lampOnObject != null) lampOnObject.SetActive(false);
        if (lampOffObject != null) lampOffObject.SetActive(true);
    }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            // 토글
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
