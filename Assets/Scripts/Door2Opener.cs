using UnityEngine;
using TMPro;  // TMP ���

[RequireComponent(typeof(Animation))]
public class Door2Opener : MonoBehaviour
{
    [Header("References")]
    public Transform player;           // �÷��̾� Transform
    public TMP_Text promptText;        // TMP �ؽ�Ʈ

    [Header("Settings")]
    public float interactDistance = 3f; // ���̶� �÷��̾� ��ġ

    private Animation anim;
    private bool isOpen = false;

    void Start()
    {
        anim = GetComponent<Animation>();

        if (promptText != null)
            promptText.gameObject.SetActive(false); // ���� ó�� ȭ��� �Ⱥ��̰�
    }

    void Update()
    {
        if (promptText == null || player == null)
        {
            return;
        }

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= interactDistance)
        {
            promptText.gameObject.SetActive(true);
            promptText.text = isOpen ? "[E] �� �ݱ�" : "[E] �� ����";

            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
                anim.Play(isOpen ? "Door2_Open" : "Door2_Close");
                promptText.text = isOpen ? "[E] �� �ݱ�" : "[E] �� ����";
            }
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }
}