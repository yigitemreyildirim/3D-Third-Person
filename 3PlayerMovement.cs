using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Hız Ayarları")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;
    public float jumpForce = 5f;

    [Header("Referanslar")]
    public Transform cameraTransform;

    [HideInInspector] public bool isCrouching; // EnemyVision burayı okuyacak

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        CheckGround();
        Move();
        Jump();
        Crouch();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");


        Vector3 moveDir = (cameraTransform.forward * v + cameraTransform.right * h);
        moveDir.y = 0f;
        moveDir.Normalize();

        float currentSpeed = isCrouching ? crouchSpeed : (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);

        if (moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);

            Vector3 move = moveDir * currentSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    bool isGrounded;
    void CheckGround()
    {
        // Kapsül yüksekliği ~2 ise 1.1 iyidir; gerekirse arttır/azalt
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            transform.localScale = new Vector3(1f, 0.5f, 1f);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
