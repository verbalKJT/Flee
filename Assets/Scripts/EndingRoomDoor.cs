using UnityEngine;

public class EndingRoomDoor : MonoBehaviour
{
    private Transform player;
    public float interactDistance = 3f;
    public AudioSource pianoMusic;

    private bool isOpened = false;

    void Start()
    {
        var pianoObj = GameObject.FindObjectOfType<InteractablePiano>();
        if (pianoObj != null)
        {
            pianoObj.pianoMusic = pianoMusic;
        }
    }
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            if (pianoMusic != null && !pianoMusic.isPlaying)
            {
                pianoMusic.Play();
                isOpened = true;
            }
        }
    }

    public void SetupPlayerTransform(Transform player)
    {
        this.player = player;
    }
}