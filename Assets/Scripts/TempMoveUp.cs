using UnityEngine;

public class TempMoveUp : MonoBehaviour
{
    float moveSpeed = 5f;

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
    }
}
