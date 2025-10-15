using UnityEngine;

public class CrosshairInteractor : MonoBehaviour
{
    public GameObject cam;
    //��ȣ�ۿ�,���� Ray�Ÿ� 
    public float interactDistance = 3f;
    public float aimDistance = 100f;
    //��ȣ�ۿ�(�ݱ�)�� , ���ӿ�(����) ������
    public LayerMask ObjectInteractLayer;
    public LayerMask AimInteractLayer;

    private IInteractable current;
    public  static Vector3 hitPoint;

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        
        current = null;

        //���ؿ� Ray
        if (Physics.Raycast(ray, out hit, aimDistance, AimInteractLayer))
        {
            hitPoint = hit.point;
        }

        //��ȣ�ۿ�� Ray
        if (Physics.Raycast(ray, out hit, interactDistance, ObjectInteractLayer))
        {
            Debug.Log("Ray hit: " + hit.collider.name);            

            current = hit.collider.GetComponent<IInteractable>();

            if (Input.GetKeyDown(KeyCode.E)&&current!=null)
            {                        
                current.Interact(); // ���� IInteractable ������Ʈ�� ��ȣ�ۿ�               
            }
        }
    }

    void OnGUI()
    {
        if (current != null)
        {
            GUI.Label(
                new Rect(Screen.width / 2 - 50, Screen.height / 2 + 30, 100, 25),
                current.GetPromptText()
            );
        }
    }
}
