using UnityEngine;

public class PlaneCollision : MonoBehaviour
{
    public GameObject explosionPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plane") || (other.CompareTag("Helicopter")))
        {
            Debug.Log("blowing up");
            Instantiate(explosionPrefab, transform.position, transform.rotation);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
