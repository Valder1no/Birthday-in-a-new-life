using UnityEngine;

public class DoorOpenWithKey : MonoBehaviour
{
    private bool shouldMove = false;
    public float moveSpeed = 2f;
    public float maxHeight = 100f;

    private float initialY;

    void Start()
    {
        initialY = transform.position.y;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            Destroy(other.gameObject);
            shouldMove = true;
        }
    }

    void Update()
    {
        if (shouldMove)
        {
            Vector3 pos = transform.position;
            if (pos.y < initialY + maxHeight)
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            }
            else
            {
                shouldMove = false;
            }
        }
    }
}
