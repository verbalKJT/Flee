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
        bool isInteracting = Input.GetKeyDown(KeyCode.E) || 
                             ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch);
        if (player == null) return; // ✅ Null 체크 추가
        
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance && !isOpened && isInteracting)
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