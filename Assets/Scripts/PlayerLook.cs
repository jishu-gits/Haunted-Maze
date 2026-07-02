using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTarget;

    [Header("Settings")]
    public float mouseSensitivity = 200f;

    public float minPitch = -80f;
    public float maxPitch = 80f;

    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate Player (Yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate Camera (Pitch)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTarget.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}