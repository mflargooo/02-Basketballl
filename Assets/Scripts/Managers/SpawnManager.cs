using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private int maxPickups;
    private GameObject[] spawned;

    void Start()
    {
        spawned = new GameObject[maxPickups];
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < maxPickups; i++)
        {
            if (!spawned[i]) spawned[i] = SpawnNewPickup();
        }
    }

    private GameObject SpawnNewPickup()
    {
        float angle = Random.Range(0f, 360f);
        float dist = Random.Range(minRadius, maxRadius);
        Vector3 spawnPos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * dist;

        return Instantiate(pickupPrefab, spawnPos, transform.rotation);
    }
}
