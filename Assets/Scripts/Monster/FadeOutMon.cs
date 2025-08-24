using UnityEngine;

public class FadeOutMon : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
    }
}
