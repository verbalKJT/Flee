using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorOpener : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            animator.SetTrigger("Door1Open");
            animator.SetTrigger("Door2Open");
        }
        else
        {
            animator.SetTrigger("Door1Close");
            animator.SetTrigger("Door2Close");
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
