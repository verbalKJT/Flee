using UnityEngine;

public class SoundHold : MonoBehaviour
{
    [SerializeField] private PlayableDirector pd;
    [SerializeField] private AudioSource audioSource;
    
   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pd != null)
        {
            pd.stopped += OnTimelineStopped;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void OnTimelineStopped(PlayableDirector director)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
    
}
