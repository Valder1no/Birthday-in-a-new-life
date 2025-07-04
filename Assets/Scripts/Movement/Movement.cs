using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float verticalSpeed = 4f;
    public float acceleration = 5f;

    public BalloonInflate leftBalloon;
    public BalloonInflate rightBalloon;

    private Vector3 currentVelocity = Vector3.zero;

    // Wind variables
    private float windStrength = 0f;
    public float maxWindForce = 2f;
    public float windChangeInterval = 5f;
    private float windChangeTimer = 0f;

    //wind arrors
    public GameObject windArrowRightPrefab;
    public GameObject windArrowLeftPrefab;
    private GameObject currentWindArrow;

    private AudioSource audioSource;

    void Update()
    {
        UpdateWind();

        // Input
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) input.x = -1;
        if (Input.GetKey(KeyCode.RightArrow)) input.x = 1;
        if (Input.GetKey(KeyCode.UpArrow)) input.y = 1;
        if (Input.GetKey(KeyCode.DownArrow)) input.y = -1;

        input = input.normalized;

        // Apply wind to movement
        float windEffect = windStrength;
        Vector3 targetVelocity = new Vector3(input.x * moveSpeed + windEffect, input.y * verticalSpeed, 0f);
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * acceleration);
        transform.position += currentVelocity * Time.deltaTime;

        // Balloon inflation
        float verticalFactor = 0.5f; // less vertical contribution
        float leftInflate = Mathf.Clamp01((1 - input.x) / 2f + input.y * verticalFactor);
        float rightInflate = Mathf.Clamp01((1 + input.x) / 2f + input.y * verticalFactor);

        if (leftBalloon != null) leftBalloon.Inflate(leftInflate);
        if (rightBalloon != null) rightBalloon.Inflate(rightInflate);
    }

    void UpdateWind()
    {
        windChangeTimer -= Time.deltaTime;
        if (windChangeTimer <= 0f)
        {
            windChangeTimer = windChangeInterval;
            windStrength = Random.Range(-maxWindForce, maxWindForce);
            Debug.Log("Wind changed to: " + windStrength);

            if (currentWindArrow != null)
            {
                Destroy(currentWindArrow);
            }

            if (windStrength > 0.1f)
            {
                currentWindArrow = Instantiate(windArrowRightPrefab, transform);

            }
            else if (windStrength < -0.1f)
            {
                currentWindArrow = Instantiate(windArrowLeftPrefab, transform);
            }

            currentWindArrow.transform.localPosition = new Vector3(0, -1, -1.5f);

        }
    }
}
