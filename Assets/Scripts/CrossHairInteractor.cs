using UnityEngine;

public class CrosshairInteractor : MonoBehaviour
{
    public GameObject cam;
    //상호작용,에임 Ray거리 
    public float interactDistance = 3f;
    public float aimDistance = 100f;
    //상호작용(줍기)용 , 에임용(놓기) 나누기
    public LayerMask ObjectInteractLayer;
    public LayerMask AimInteractLayer;

    private IInteractable current;
    public  static Vector3 hitPoint;

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        
        current = null;

        //조준용 Ray
        if (Physics.Raycast(ray, out hit, aimDistance, AimInteractLayer))
        {
            hitPoint = hit.point;
        }

        //상호작용용 Ray
        if (Physics.Raycast(ray, out hit, interactDistance, ObjectInteractLayer))
        {
            Debug.Log("Ray hit: " + hit.collider.name);            

            current = hit.collider.GetComponent<IInteractable>();

            if (Input.GetKeyDown(KeyCode.E)&&current!=null)
            {                        
                current.Interact(); // 기존 IInteractable 오브젝트와 상호작용               
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
