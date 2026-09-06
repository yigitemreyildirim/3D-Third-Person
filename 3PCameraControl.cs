using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Takip edilecek karakter
    public Vector2 sensitivity = new Vector2(100f, 100f);
    public float distance = 3.0f; // Karakter ile kamera arası
    public float yMin = -30f;
    public float yMax = 60f;
    public float smoothTime = 0.1f;

    private float yaw;
    private float pitch;
    private Vector3 currentVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, yMin, yMax);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        Quaternion rotation = Quaternion.Euler(targetRotation);

        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
        transform.rotation = rotation;
    }
}

