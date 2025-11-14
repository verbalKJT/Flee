using UnityEngine;
using System.Collections;

public class RightHandGrabber : MonoBehaviour
{
    public LayerMask grabbableLayer;
    public float grabRange = 0.2f;
    public float throwPower = 10f;
    public float rotPower = 5f;

    public bool isRemoteGrab = true;
    public float remoteGrabDistance = 10f;

    private IGrabbable grabbed;
    private GameObject grabbedObject;
    private Vector3 prevPos;
    private Quaternion prevRot;

    void Update()
    {
        if (grabbed == null)
            TryGrab();
        else
            TryRelease();
    }

    void TryGrab()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            GameObject target = null;

            // 원거리 물체 잡기를 사용한다면 
            if (isRemoteGrab)
            {
                Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
                if (Physics.SphereCast(ray, 0.3f, out RaycastHit hit, remoteGrabDistance, grabbableLayer))
                {
                    target = hit.collider.gameObject;
                    StartCoroutine(RemoteGrab(target));
                    return;
                }
            }
            // 영역 안에 있는 모든 잡기 옵젝 검출
            Collider[] hits = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange, grabbableLayer);
            if (hits.Length > 0)
            {
                target = hits[0].gameObject;
            }

            if (target != null && target.TryGetComponent<IGrabbable>(out var grabbable))
            {
                grabbed = grabbable;
                grabbedObject = target;
                grabbed.Grab(ARAVRInput.RHand);
                prevPos = ARAVRInput.RHandPosition;
                prevRot = ARAVRInput.RHand.rotation;
            }
        }
    }

    void TryRelease()
    {
        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            if (grabbedObject.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = false;
                grabbedObject.transform.parent = null;

                Vector3 throwDir = (ARAVRInput.RHandPosition - prevPos);
                rb.linearVelocity = ARAVRInput.RHandDirection * throwPower;

                Quaternion deltaRot = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
                deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
                rb.angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            }

            grabbed.Release();
            grabbed = null;
            grabbedObject = null;
        }
    }

    IEnumerator RemoteGrab(GameObject target)
    {
        if (!target.TryGetComponent<IGrabbable>(out var grabbable)) yield break;

        grabbed = grabbable;
        grabbedObject = target;

        if (target.TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;

        Vector3 start = target.transform.position;
        Vector3 end = ARAVRInput.RHandPosition + ARAVRInput.RHandDirection * 0.1f;
        float t = 0;
        float duration = 0.2f;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            target.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        grabbed.Grab(ARAVRInput.RHand);
        prevPos = ARAVRInput.RHandPosition;
        prevRot = ARAVRInput.RHand.rotation;
    }
}
