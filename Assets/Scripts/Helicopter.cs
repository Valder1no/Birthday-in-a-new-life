using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterSpawner : MonoBehaviour
{
    public GameObject[] planePrefabs;

    public Vector3 spawnCenter = Vector3.zero;
    public Vector3 spawnSize = new Vector3(0, 10, 0);

    public float moveSpeed = 4f;

    public float minSpawnInterval = 6f;
    public float maxSpawnInterval = 8f;

    public float spawnYOffset = 35f;

    public GameObject camera;

    private List<GameObject> spawnedPlanes = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnHelicoptersWithInterval());
    }

    IEnumerator SpawnHelicoptersWithInterval()
    {
        while (true)
        {
            Vector3 randomPosition = spawnCenter + new Vector3(
                Random.Range(-spawnSize.x / 2, spawnSize.x / 2),
                Random.Range(-spawnSize.y / 2, spawnSize.y / 2),
                Random.Range(-spawnSize.z / 2, spawnSize.z / 2)
            );

            GameObject randomPlanePrefab = planePrefabs[Random.Range(0, planePrefabs.Length)];

            GameObject plane = Instantiate(randomPlanePrefab, randomPosition, randomPlanePrefab.transform.rotation);
            spawnedPlanes.Add(plane);

            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);
            Destroy(plane, 20f);
        }
    }

    void Update()
    {
        if (camera != null)
        {
            spawnCenter = new Vector3(
                spawnCenter.x,
                camera.transform.position.y + spawnYOffset,
                spawnCenter.z
            );
        }

        foreach (GameObject plane in spawnedPlanes)
        {
            if (plane != null)
            {
                plane.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnCenter, spawnSize);
    }
}
