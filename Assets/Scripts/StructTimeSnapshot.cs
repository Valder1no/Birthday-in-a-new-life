using UnityEngine;

[System.Serializable]
public struct TimeSnapshot
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;
    public bool isActive;

    // Add these:
    public bool shouldMove;
    public bool shouldMoveDown;

    public bool? enemyShouldIBeAlive;

    public TimeSnapshot(Rigidbody rb, bool shouldMove = false, bool shouldMoveDown = false, bool? enemyShouldIBeAlive = null)
    {
        position = rb.position;
        rotation = rb.rotation;
        velocity = rb.linearVelocity;
        angularVelocity = rb.angularVelocity;
        isActive = rb.gameObject.activeSelf;

        this.shouldMove = shouldMove;
        this.shouldMoveDown = shouldMoveDown;

        this.enemyShouldIBeAlive = enemyShouldIBeAlive;
    }
}
