using UnityEngine;

[RequireComponent(typeof(Animation))]
public class EndingRoomDoor : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 3f;
    public AudioSource pianoMusic; // piano music
    private Animation anim;
    private bool isOpen = false;
    private bool hasPlayedMusic = false;

    void Start()
    {
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= interactDistance && !isOpen && Input.GetKeyDown(KeyCode.E))
        {
            anim.Play("Door2_Open");
            isOpen = true;

            if (!hasPlayedMusic && pianoMusic != null)
            {
                pianoMusic.Play();
                hasPlayedMusic = true;
            }
        }
    }
}
