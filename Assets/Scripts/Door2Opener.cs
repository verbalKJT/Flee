using UnityEngine;
using TMPro;  // TMP ���

[RequireComponent(typeof(Animation))]
public class Door2Opener : MonoBehaviour
{
    [Header("References")]
    public Transform player;           // �÷��̾� Transform
     // TMP �ؽ�Ʈ

    [Header("Settings")]
    public float interactDistance = 3f; // ���̶� �÷��̾� ��ġ

    private Animation anim;
    private bool isOpen = false;

    void Start()
    {
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
                anim.Play(isOpen ? "Door2_Open" : "Door2_Close");
            }
        }  
    }
}