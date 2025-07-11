using System;
using System.Collections;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    private Light targetLight;
    private float lightIntensity;
    [SerializeField] private float interactionDistance = 3f; // 상호작용 거리
    [SerializeField] private Transform player; // 플레이어 트랜스폼
    private float distance;
    private bool inRange,isTurn = true;
    [SerializeField] private GameObject interactionUI; // UI
    void Start()
    {
        targetLight = GetComponent<Light>();
        lightIntensity = targetLight.intensity;
        StartCoroutine(TurnLight());
    }

    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        inRange = distance <= interactionDistance;
        if (inRange)
        {
            interactionUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && isTurn)
            {
                targetLight.intensity = 0f;
                StopCoroutine(TurnLight());
                isTurn = false;
            }
            else if (Input.GetKeyDown(KeyCode.E) && !isTurn)
            {
                targetLight.intensity = lightIntensity;
                StopCoroutine(TurnLight());
                isTurn = true;
            }
        }
        else
        {
            interactionUI.SetActive(false);
        }
    }

    private IEnumerator TurnLight()
    {
        while (true)
        {
            targetLight.intensity = 0f;
            yield return new WaitForSeconds(0.5f);

            targetLight.intensity = lightIntensity;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
