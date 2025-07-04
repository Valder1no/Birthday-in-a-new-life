using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class DoorOpenWithKey : MonoBehaviour
{
    private bool shouldMove = false;
    private bool shouldMoveDown = false;
    public float moveSpeed = 2f;
    public float maxHeight = 140f;

    public bool IsMovingUp() => shouldMove;
    public bool IsMovingDown() => shouldMoveDown;

    public bool openableWithKey;

    private bool shouldMoveUpDown = false;

    private float initialY;

    void Start()
    {
        initialY = transform.position.y;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            if (openableWithKey) 
            {
                other.gameObject.SetActive(false);
                shouldMove = true;
            }
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

    public void moveUpDown() 
    {
        StartCoroutine(MoveUpThenDown());
    }

    public IEnumerator MoveUpThenDown()
    {
        float targetY = transform.position.y + maxHeight;

        // Move up
        while (transform.position.y < targetY)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            yield return null; // wait until next frame
        }

        // Snap to exact height
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);

        // Optional: small wait at the top
        yield return new WaitForSeconds(0.5f);

        // Move down
        while (transform.position.y > initialY)
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // Snap to initial height
        transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
    }



    void Update()
    {
        if (shouldMove)
        {
            Vector3 pos = transform.position;
            if (pos.y < initialY + maxHeight)
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            else
                shouldMove = false;
        }

        if (shouldMoveDown)
        {
            Vector3 pos = transform.position;
            if (pos.y > initialY)
                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            else
                shouldMoveDown = false;
        }
    }




}
