using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 ����� ����
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 30f); // �� �Ʒ� ���� ����

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);      // ī�޶� ȸ��
        playerBody.Rotate(Vector3.up * mouseX);                             // ��ü �¿� ȸ��

    }
}
