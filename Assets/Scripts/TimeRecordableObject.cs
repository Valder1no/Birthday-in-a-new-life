using UnityEngine;
using System.Collections.Generic;

public class TimeRecordableObject : MonoBehaviour, ITimeRecordable
{
    private Rigidbody rb;
    private List<TimeSnapshot> snapshots = new List<TimeSnapshot>();

    private TimeSnapshot backupSnapshot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        TimeCheckpointManager.Instance.Register(this);
        Debug.Log($"Registered!!!!");
    }

    public void RecordSnapshot()
    {
        snapshots.Add(new TimeSnapshot(rb));
        Debug.Log($"{gameObject.name} snapshot recorded. Total: {snapshots.Count}");
    }

    public void ApplySnapshot(TimeSnapshot snapshot)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = snapshot.position;
        rb.rotation = snapshot.rotation;
        rb.linearVelocity = snapshot.velocity;
        rb.angularVelocity = snapshot.angularVelocity;

        if (TryGetComponent<FirstPersonController>(out var controller))
        {
            // Reset internal velocity so it doesn't conflict with rewind
            controller.ResetVelocity(snapshot.velocity);
        }

    }

    public void SetBackupSnapshot(TimeSnapshot snapshot)
    {
        backupSnapshot = snapshot;
    }

    public TimeSnapshot GetBackupSnapshot()
    {
        return backupSnapshot;
    }

    public List<TimeSnapshot> GetSnapshots() => snapshots;
    public void ClearSnapshots() => snapshots.Clear();
}
