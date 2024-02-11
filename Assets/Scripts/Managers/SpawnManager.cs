using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private int basePickups = 3;
    [SerializeField] private int addPickupsPerPlayers = 2;

    private int maxPickups;
    private static int livePickupCount = 0;
    void Start()
    {
        livePickupCount = 0;
        maxPickups = basePickups + addPickupsPerPlayers * (GameInfo.playerIndices.Count - 1);
    }
    // Update is called once per frame
    void Update()
    {
        if (livePickupCount < maxPickups)
        {
            SpawnNewPickup();
            livePickupCount++;
        }
    }

    private GameObject SpawnNewPickup()
    {
        float angle = Random.Range(0f, 360f);
        float dist = Random.Range(minRadius, maxRadius);
        Vector3 spawnPos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * dist + Vector3.up * spawnCenter.position.y;

        return Instantiate(pickupPrefab, spawnPos, transform.rotation);
    }

    public static void DecrementPickup()
    {
        livePickupCount--;
    }
}
