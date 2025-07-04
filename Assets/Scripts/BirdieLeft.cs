using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class BirdieLeft : MonoBehaviour
{
    public GameObject planePrefab;

    public Vector3 spawnCenter = Vector3.zero;
    public Vector3 spawnSize = new Vector3(0, 10, 0);

    public float moveSpeed;

    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;

    public float spawnYOfsset = 25f;

    public GameObject camera;

    private List<GameObject> spawnedPlanes = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnPlanesWithInterval());
    }

    IEnumerator SpawnPlanesWithInterval()
    {
        while (true)
        {
            Vector3 randomPosition = spawnCenter + new Vector3(
                Random.Range(-spawnSize.x / 2, spawnSize.x / 2),
                Random.Range(-spawnSize.y / 2, spawnSize.y / 2),
                Random.Range(-spawnSize.z / 2, spawnSize.z / 2)
            );

            GameObject plane = Instantiate(planePrefab, randomPosition, transform.rotation);
            spawnedPlanes.Add(plane);

            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);

            Destroy(plane, 8f);
        }
    }

    void Update()
    {
        if (camera != null)
        {
            spawnCenter = new Vector3(
                spawnCenter.x,
                camera.transform.position.y + spawnYOfsset,
                spawnCenter.z
            );
        }

        foreach (GameObject plane in spawnedPlanes)
        {
            moveSpeed = Random.Range(50f, 60f);
            if (plane != null)
            {
                plane.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnCenter, spawnSize);
    }
}
