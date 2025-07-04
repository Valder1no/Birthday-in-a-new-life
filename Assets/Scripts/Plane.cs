using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    public GameObject[] planePrefabs;

    public Vector3 spawnCenter = Vector3.zero;
    public Vector3 spawnSize = new Vector3(0, 10, 0);

    public float moveSpeed;

    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;

    public float spawnYOfsset = 25f;

    public GameObject camera;

    public GameObject smokePrefab;

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

            int randomIndex = Random.Range(0, planePrefabs.Length);
            GameObject randomPlanePrefab = planePrefabs[randomIndex];

            GameObject plane = Instantiate(randomPlanePrefab, randomPosition, randomPlanePrefab.transform.rotation);
            spawnedPlanes.Add(plane);

            Vector3 smokeOffset;
            if (randomIndex == 0)
            {
                smokeOffset = new Vector3(0, 0, -3); 
            }
            else
            {
                smokeOffset = new Vector3(0, 4, -3); 
            }

            GameObject smoke = Instantiate(smokePrefab, plane.transform);
            smoke.transform.localPosition = smokeOffset;
            smoke.transform.rotation = plane.transform.rotation * Quaternion.Euler(0, 180f, 0);

            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);

            Destroy(plane, 16f);
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
            moveSpeed = Random.Range(7f, 36f);
            if (plane != null)
            {
                plane.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnCenter, spawnSize);
    }
}
