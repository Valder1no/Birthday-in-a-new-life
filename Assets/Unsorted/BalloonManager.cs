using UnityEngine;
using System.Collections.Generic;

public class BalloonManager : MonoBehaviour
{
    public static BalloonManager Instance;

    public Rigidbody objectToPull;
    public float maxPullForce = 50f;
    public float minForce = 10f;  // NEW: base force even for small size difference
    public float responseMultiplier = 100f; // NEW: scales the force more aggressively

    private List<BalloonBehavior> balloons = new List<BalloonBehavior>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterBalloon(BalloonBehavior balloon)
    {
        if (!balloons.Contains(balloon))
            balloons.Add(balloon);
    }

    void FixedUpdate()
    {
        if (objectToPull == null || balloons.Count < 2)
            return;

        BalloonBehavior leftBalloon = balloons[0];
        BalloonBehavior rightBalloon = balloons[1];

        float leftSize = leftBalloon.GetScaleMagnitude();
        float rightSize = rightBalloon.GetScaleMagnitude();
        float sizeDiff = Mathf.Abs(leftSize - rightSize);

        if (sizeDiff < 0.01f)
            return;

        Vector3 direction;
        if (leftSize > rightSize)
        {
            direction = (leftBalloon.GetPosition() - objectToPull.position).normalized;
        }
        else
        {
            direction = (rightBalloon.GetPosition() - objectToPull.position).normalized;
        }

        direction.y = 0; // keep horizontal only

        // More responsive force: size difference scaled
        float forceStrength = Mathf.Clamp(minForce + (sizeDiff * responseMultiplier), 0, maxPullForce);
        objectToPull.AddForce(direction * forceStrength, ForceMode.Force);
    }
}