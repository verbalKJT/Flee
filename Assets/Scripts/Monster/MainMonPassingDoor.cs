using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MainMonPassingDoor : MonoBehaviour
{
    [SerializeField] private NavMeshObstacle navMeshObstacle;
    private Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "MainMon")
        {
            StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        anim.Play("Door2_Open");
        GetComponent<BoxCollider>().enabled = false;
        navMeshObstacle.enabled = false;
        yield return new WaitForSeconds(1.0f); // 문 열리는 시간 대기
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == "MainMon")
        {
            anim.Play("Door2_Open");
            navMeshObstacle.enabled = true;
        }
    }
}
