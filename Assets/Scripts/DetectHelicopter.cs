using UnityEngine;

public class HelicopterAttractor : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float pullSpeed = 2f;
    public string playerTag = "Player";

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(playerTag))
            {
                Debug.Log("Pulling player with Translate");
                Transform playerTransform = hit.transform;
                Vector3 directionToHelicopter = (transform.position - playerTransform.position).normalized;

                playerTransform.Translate(directionToHelicopter * pullSpeed * Time.deltaTime);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
