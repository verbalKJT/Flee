using UnityEngine;

[RequireComponent(typeof(Animation), typeof(Collider))]
public class DoorOpener : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 10f;
    public string openClip = "Door2_Open";
    public string closeClip = "Door2_Close";

    private Animation anim;
    private bool isOpen = false;

    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play(openClip);

    }
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) > interactDistance)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            anim.Play(isOpen ? openClip : closeClip);
        }
    }
}