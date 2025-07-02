using System.Collections;
using UnityEngine;

public class DoorOpenWithKey : MonoBehaviour
{
    private bool shouldMove = false;
    private bool shouldMoveDown = false;
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

    public void moveUp()
    {
        shouldMove = true;
    }

    public void moveDown()
    {
        shouldMoveDown = true;
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

        if (shouldMoveDown)
        {
            Vector3 pos = transform.position;
            if (pos.y > initialY)
            {
                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            }
            else
            {
                shouldMoveDown = false;
            }
        }
    }



}
