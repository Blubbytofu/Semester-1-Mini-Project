using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject lookTargetPosition;
    [SerializeField] private GameObject lookTargetObject;

    private float mouseX;
    private float mouseY;
    private float xRotation;
    private float yRotation;
    [SerializeField] private float xSensitivity = 2f;
    [SerializeField] private float ySensitivity = 2f;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerCamera = GameObject.Find("Main Camera");
        lookTargetPosition = GameObject.Find("Head Aim Position");
        lookTargetObject = GameObject.Find("Target");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        lookTargetObject.transform.position = lookTargetPosition.transform.position;
    }

    private void LateUpdate()
    {
        MouseLook();
    }

    private void MouseLook()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * xSensitivity;
        xRotation -= mouseY * ySensitivity;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        player.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}