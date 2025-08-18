using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectDistrict : MonoBehaviour
{
    public GameObject uiImage;         // ������ UI �̹��� ������Ʈ
    public float displayTime = 2f;     // �������� �ð�
    public AudioClip screamSound;      // ��� ���� Ŭ��

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

