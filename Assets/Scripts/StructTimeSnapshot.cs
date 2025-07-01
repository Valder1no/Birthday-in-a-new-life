// TimeSnapshotStruct.cs
using UnityEngine;

[System.Serializable]
public struct TimeSnapshot
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    public TimeSnapshot(Rigidbody rb)
    {
        position = rb.position;
        rotation = rb.rotation;
        velocity = rb.linearVelocity;
        angularVelocity = rb.angularVelocity;
    }
}