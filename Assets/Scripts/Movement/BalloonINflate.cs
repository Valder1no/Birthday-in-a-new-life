using UnityEngine;

public class BalloonInflate : MonoBehaviour
{
    public Vector3 deflatedScale = new Vector3(1, 1, 1);
    public Vector3 inflatedScale = new Vector3(1.5f, 1.5f, 1.5f);
    public float inflateSpeed = 5f;

    private float targetInflateAmount = 0f;

    public void Inflate(float amount)
    {
        targetInflateAmount = Mathf.Clamp01(amount);
    }

    void Update()
    {
        Vector3 targetScale = Vector3.Lerp(deflatedScale, inflatedScale, targetInflateAmount);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * inflateSpeed);
    }
}
