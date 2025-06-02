using UnityEngine;
using TMPro;  // TMP 사용

[RequireComponent(typeof(Animation))]
public class Door1Opener : MonoBehaviour
{
    [Header("References")]
    public Transform player;           // 플레이어 Transform
    public TMP_Text promptText;        // TMP 텍스트

    [Header("Settings")]
    public float interactDistance = 3f; // 문이랑 플레이어 위치

    private Animation anim;
    private bool isOpen = false;

    void Start()
    {
        anim = GetComponent<Animation>();

        if (promptText != null)
            promptText.gameObject.SetActive(false); // 시작 시 텍스트 숨김
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position); // 플레이어랑 거리

        if (dist <= interactDistance)
        {
            // 텍스트 표시
            if (promptText != null)
            {
                promptText.gameObject.SetActive(true);
                promptText.text = isOpen ? "[E] 문 닫기" : "[E] 문 열기";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
                anim.Play(isOpen ? "Door1_Open" : "Door1_Close");
            }
        }
        else
        {
            // 거리 밖이면 텍스트 숨김
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
    }
}

