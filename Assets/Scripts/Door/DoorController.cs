using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animation anim;
    private bool isPlayerNear = false;
    private bool isOpen = false;

    public string openAnimName = "Door1_Open";
    public string closeAnimName = "Door1_Close";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen)
                anim.Play(closeAnimName);
            else
                anim.Play(openAnimName);

            isOpen = !isOpen;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = false;
    }
}
