using UnityEngine;
using TMPro;  // TMP 사용

[RequireComponent(typeof(Animation))]
public class Door1Opener : MonoBehaviour
{
    [Header("References")]
    public Transform player;           // 플레이어 Transform
      // TMP 텍스트

    [Header("Settings")]
    public float interactDistance = 3f; // 문이랑 플레이어 위치

    private Animation anim;
    private bool isOpen = false;

    void Start()
    {
        anim = GetComponent<Animation>();

    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position); // 플레이어랑 거리

        if (dist <= interactDistance) // 거리 안으로 들어왔을때
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
                anim.Play(isOpen ? "Door1_Open" : "Door1_Close"); // 문 열기 뜸과 동시에 문 열림
            }
        }
    }
}

