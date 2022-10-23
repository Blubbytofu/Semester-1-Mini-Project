using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraPosition;
    [SerializeField] private GameObject lookTargetPosition;
    [SerializeField] private GameObject lookTargetObject;
    [SerializeField] private GameManager gameManager;

    private float mouseX;
    private float mouseY;
    private float xRotation;
    private float yRotation;

    [SerializeField] private float xSensitivity = 2f;
    [SerializeField] private float ySensitivity = 2f;

    private void Start()
    {
        player = GameObject.Find("Player");
        cameraPosition = GameObject.Find("Camera Position");
        lookTargetPosition = GameObject.Find("Head Aim Position");
        lookTargetObject = GameObject.Find("Target");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        transform.position = cameraPosition.transform.position;
        lookTargetObject.transform.position = lookTargetPosition.transform.position;
    }

    private void LateUpdate()
    {
        if (gameManager.universalGameOver)
        {
            transform.rotation = cameraPosition.transform.rotation;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            MouseLook();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void MouseLook()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * xSensitivity;
        xRotation -= mouseY * ySensitivity;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        player.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}