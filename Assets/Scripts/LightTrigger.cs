using System;
using System.Collections;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    private Light targetLight;
    private float lightIntensity;
    void Start()
    {
        targetLight = GetComponent<Light>();
        lightIntensity = targetLight.intensity;
        StartCoroutine(TurnLight());
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
