using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCamera;

    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private Vector3 rewindVelocity = Vector3.zero;
    private bool applyRewindVelocity = false;

    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    private float gravityLockTimer = 0f;
    private Vector3 nextFrameVelocity = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Gravity
        if (gravityLockTimer > 0f)
        {
            gravityLockTimer -= Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (applyRewindVelocity)
        {
            controller.Move(rewindVelocity * Time.deltaTime);
            applyRewindVelocity = false;
        }

        controller.Move(velocity * Time.deltaTime);
        Debug.Log($"Rewind velocity: {rewindVelocity}");

        // Apply any one-time rewind velocity
        if (nextFrameVelocity != Vector3.zero)
        {
            controller.Move(nextFrameVelocity * Time.deltaTime);
            nextFrameVelocity = Vector3.zero;
        }

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void SnapToRewindState(Vector3 pos, Quaternion rot, Vector3 vel)
    {
        controller.enabled = false;
        transform.position = pos;
        transform.rotation = rot;

        rewindVelocity = vel;
        applyRewindVelocity = true;
        Debug.Log($"Player position after re-enabling controller: {transform.position}");

        // Optional: disable gravity for a brief moment to prevent snap-down
        gravityLockTimer = 0.2f;

        controller.enabled = true;
    }

    public void ResetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }

    public void LockGravity(float duration)
    {
        gravityLockTimer = duration;
    }

    public void ApplyNextVelocity(Vector3 vel)
    {
        nextFrameVelocity = vel;
    }
}
