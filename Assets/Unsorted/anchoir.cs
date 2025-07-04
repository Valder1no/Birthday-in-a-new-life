using UnityEngine;

public class BalloonAnchor : MonoBehaviour
{
    public Transform targetObject;
    public float xOffset = -1f;
    public float yOffset = 2f;
    public float zOffset = 0f;
    public float followForce = 50f;     // ? Needed for movement
    public float maxSpeed = 5f;
    public float uprightTorqueStrength = 50f; // ? Needed for stabilization

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("BalloonAnchor requires a Rigidbody.");
        }
    }

    void FixedUpdate()
    {
        if (targetObject == null || rb == null) return;

        // Move to target relative position
        Vector3 targetPos = targetObject.position + new Vector3(xOffset, yOffset, zOffset);
        Vector3 direction = (targetPos - transform.position);
        Vector3 desiredVelocity = direction * followForce;

        if (desiredVelocity.magnitude > maxSpeed)
            desiredVelocity = desiredVelocity.normalized * maxSpeed;

        rb.linearVelocity = desiredVelocity;

        // Keep upright
        ApplyUprightTorque();
    }

    void ApplyUprightTorque()
    {
        Quaternion currentRot = rb.rotation;
        Quaternion targetRot = Quaternion.Euler(0, currentRot.eulerAngles.y, 0); // Preserve Y rotation

        Quaternion deltaRot = targetRot * Quaternion.Inverse(currentRot);
        deltaRot.ToAngleAxis(out float angle, out Vector3 axis);

        if (angle > 180f) angle -= 360f;
        Vector3 torque = axis * (angle * Mathf.Deg2Rad * uprightTorqueStrength);

        rb.AddTorque(torque);
    }
}