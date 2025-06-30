using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TimeRecordable : MonoBehaviour, ITimeRecordable
{
    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private Vector3 savedVelocity;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        TimeCheckpointManager.Instance.Register(this);
    }

    public void SaveCheckpoint()
    {
        savedPosition = transform.position;
        savedRotation = transform.rotation;
        savedVelocity = rb.linearVelocity;
    }

    public void LoadCheckpoint()
    {
        rb.linearVelocity = Vector3.zero;
        transform.position = savedPosition;
        transform.rotation = savedRotation;
        rb.linearVelocity = savedVelocity;
    }
}
