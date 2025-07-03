using System.Collections;
using UnityEngine;

public class DoorOpenWithKey : MonoBehaviour
{
    private bool shouldMove = false;
    private bool shouldMoveDown = false;
    public float moveSpeed = 2f;
    public float maxHeight = 140f;

    public bool IsMovingUp() => shouldMove;
    public bool IsMovingDown() => shouldMoveDown;

    private float initialY;

    void Start()
    {
        initialY = transform.position.y;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
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

    public void SetMoveStates(bool move, bool moveDown)
    {
        shouldMove = move;
        shouldMoveDown = moveDown;
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
