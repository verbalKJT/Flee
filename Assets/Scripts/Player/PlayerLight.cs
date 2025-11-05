using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    private Light flashlightLight;
    private bool isOn = false;

    public AudioSource lightAudioSource;
    public AudioClip lightOnClip;
    public AudioClip lightOffClip;

    void Awake()
    {
        // 자식까지 전부 뒤져서 Light 하나 찾아옴
        flashlightLight = GetComponentInChildren<Light>(true);

        if (flashlightLight != null)
            flashlightLight.enabled = false;   // 처음엔 꺼진 상태
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;

            if (flashlightLight != null)
            {
                flashlightLight.enabled = isOn;
                if (lightAudioSource != null && lightOnClip != null &&lightOffClip != null)
                {
                    lightAudioSource.PlayOneShot(isOn ? lightOnClip : lightOffClip);
                }
            }
        }
    }
}
