using UnityEngine;

public class LeftHandGrabFlash : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private Animator leftHandAnimator;

    void Start()
    {
        leftHandAnimator = leftHand.GetComponentInChildren<Animator>();
    }
    
    public void StartAnim()
    {
        leftHandAnimator.SetBool("IsGrab", true);
    }
    
    public void SetleftHand(GameObject leftHand)
    {
        this.leftHand = leftHand;
    }
}
