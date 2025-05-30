using Unity.AI.Navigation;
using UnityEngine;

public class MainMonPassingDoor : MonoBehaviour
{
    [SerializeField] private Animator doorAnim;
    [SerializeField] private NavMeshLink navLink;

    void OnTriggerStay(Collider other)
    {
        if (other.name == "MainMon")
        {
            doorAnim.SetBool("isOpen",true);
            navLink.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "MainMon")
        {
            doorAnim.SetBool("isOpen",false);
            navLink.enabled = false;
        }
    }
}
