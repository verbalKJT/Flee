using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public Transform holdPoint;               // 손에 들고 있는 위치
    public GameObject heldObject = null;      // 현재 들고 있는 오브젝트
    public float throwForce = 10f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        // Q키로 던지기
        if (heldObject != null && Input.GetKeyDown(KeyCode.Q))
        {
            ThrowHeldObject();
        }
    }

    public void PickupToothbrush(GameObject obj)
    {
        heldObject = obj;

        // 충돌 없애기
        Collider col = obj.GetComponent<Collider>();
        if (col) col.enabled = false;

        // 물리 없애기
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        // 들고 있는 위치로 이동
        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        // (선택) UI 인벤토리 슬롯에 아이콘 표시 예정
    }

    private void ThrowHeldObject()
    {
        heldObject.transform.SetParent(null);

        // Rigidbody 다시 활성화
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        Collider col = heldObject.GetComponent<Collider>();
        if (rb)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Camera.main.transform.forward * throwForce + Vector3.up * 2f, ForceMode.VelocityChange);
        }

        if (col) col.enabled = true;

        // 사운드 유도 기능 활성화
        var emitter = heldObject.GetComponent<ThrownSoundEmitter>();
        if (emitter != null)
            emitter.enabled = true;

        heldObject = null;
    }
}