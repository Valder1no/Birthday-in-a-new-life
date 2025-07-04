using UnityEngine;

public class CameraVerticalFollow : MonoBehaviour
{
    public Transform targetToFollow; // Assign the object (e.g., player) in the Inspector
    public float followSpeed = 5f;   // Smooth follow speed

    private float initialX;
    private float initialZ;

    void Start()
    {
        if (targetToFollow == null)
        {
            Debug.LogWarning("CameraVerticalFollow: targetToFollow not assigned.");
            enabled = false;
            return;
        }

        // Store initial X and Z positions of the camera
        initialX = transform.position.x;
        initialZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (targetToFollow == null)
            return;

        Vector3 newPosition = new Vector3(
            initialX,
            Mathf.Lerp(transform.position.y, targetToFollow.position.y, followSpeed * Time.deltaTime),
            initialZ
        );

        transform.position = newPosition;
    }
}