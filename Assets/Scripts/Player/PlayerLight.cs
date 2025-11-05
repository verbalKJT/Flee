using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public Light lanternLight;
    public AudioSource lanternAudioSource;
    public AudioClip lightOnClip;
    public AudioClip lightOffClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (lanternLight != null)
        {
            lanternLight.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(lanternLight != null)
            {
                lanternLight.enabled = !lanternLight.enabled;
                
                // 효과음 재생
                if (lanternAudioSource != null && lightOnClip != null &&  lightOffClip != null)
                {
                    lanternAudioSource.PlayOneShot(lanternLight.enabled ? lightOnClip : lightOffClip);
                }
            }
        }
    }
}
