using Unity.AI.Navigation;
using UnityEngine;

public class MainMonPassingDoor : MonoBehaviour
{
    [SerializeField] private NavMeshLink navLink;

    void OnTriggerStay(Collider other)
    {
        if (other.name == "MainMon")
        {
            navLink.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "MainMon")
        {
            navLink.enabled = false;
        }
    }
}
