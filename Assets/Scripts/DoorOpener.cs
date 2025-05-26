using UnityEngine;

[RequireComponent(typeof(Animation), typeof(Collider))]
public class DoorOpener : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 10f;
    public string openClip = "Door1_Open";
    public string closeClip = "Door1_Close";

    private Animation anim;
    private bool isOpen = false;

    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play("Door1_Close");

    }
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) > interactDistance)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E 키 입력 감지됨!");
            isOpen = !isOpen;
            anim.Play(isOpen ? "Door1_Open" : "Door1_Close");
        }
    }
}