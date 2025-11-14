using UnityEngine;

public class GrabbableItem : MonoBehaviour, IGrabbable
{
    public ItemData itemData;

    public void Grab(Transform hand)
    {
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Release()
    {
        transform.SetParent(null);
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
