using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectDistrict : MonoBehaviour
{
    public GameObject uiImage;         // 보여줄 UI 이미지 오브젝트
    public float displayTime = 2f;     // 보여지는 시간
    public AudioClip screamSound;      // 비명 사운드 클립

    private AudioSource audioSource;
    private bool hasTriggered = false;

    void Start()
    {
        if (uiImage != null)
            uiImage.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (uiImage != null)
                uiImage.SetActive(true);

            if (audioSource != null && screamSound != null)
                audioSource.PlayOneShot(screamSound);

            StartCoroutine(HideUIAfterDelay(displayTime));
        }
    }

    IEnumerator HideUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (uiImage != null)
            uiImage.SetActive(false);
    }
}

