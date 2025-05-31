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
            promptText.gameObject.SetActive(false); // 먼저 처음 화면상엔 안보이게
    }

    void Update()
    {
        if (promptText == null || player == null) return;

        float dist = Vector3.Distance(player.position, transform.position); // 플레이어랑 거리

        if (dist <= interactDistance) // 거리 안으로 들어왔을때
        {
            promptText.gameObject.SetActive(true); // 문 닫기 뜸
            promptText.text = isOpen ? "[E] 문 닫기" : "[E] 문 열기";

            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
                anim.Play(isOpen ? "Door1_Open" : "Door1_Close"); // 문 열기 뜸과 동시에 문 열림
                promptText.text = isOpen ? "[E] 문 닫기" : "[E] 문 열기";
            }
        }
        else
        {
            promptText.gameObject.SetActive(false); // 거리가 멀어지면 안보이도록
        }
    }
}

