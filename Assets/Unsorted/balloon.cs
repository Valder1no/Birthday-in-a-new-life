using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BalloonBehavior : MonoBehaviour
{
    public float liftForce = 5f;
    public float maxLiftSpeed = 2f;
    public float wobbleAmount = 0.1f;
    public float wobbleSpeed = 1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // Register this balloon with the manager
        BalloonManager.Instance.RegisterBalloon(this);
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.y < maxLiftSpeed)
        {
            rb.AddForce(Vector3.up * liftForce, ForceMode.Acceleration);
        }

        Vector3 wobble = new Vector3(
            Mathf.Sin(Time.time * wobbleSpeed),
            0,
            Mathf.Cos(Time.time * wobbleSpeed)
        ) * wobbleAmount;

        rb.AddForce(wobble, ForceMode.Acceleration);
    }

    public float GetScaleMagnitude()
    {
        return transform.localScale.magnitude;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void GrowBalloon(float scaleSpeed)
    {
        transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
    }








    public void GrowBalloonWithMaxSize(float growAmount, float maxSize)
    {
        Vector3 newScale = transform.localScale + Vector3.one * growAmount * Time.deltaTime;

        float clampedX = Mathf.Min(newScale.x, maxSize);
        float clampedY = Mathf.Min(newScale.y, maxSize);
        float clampedZ = Mathf.Min(newScale.z, maxSize);

        transform.localScale = new Vector3(clampedX, clampedY, clampedZ);



    }

}

