using UnityEngine;

public class CrosshairInteractor : MonoBehaviour
{
    public GameObject cam;
    public float distance = 3f;
    public LayerMask interactLayer;

    private IInteractable current;

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        current = null;

        if (Physics.Raycast(ray, out hit, distance, interactLayer))
        {
            Debug.Log("Ray hit: " + hit.collider.name);
            current = hit.collider.GetComponent<IInteractable>();

            if (current != null && Input.GetKeyDown(KeyCode.E))
            {
                current.Interact();
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
