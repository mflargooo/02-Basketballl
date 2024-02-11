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

    [SerializeField] private GameObject[] players;

    private int maxPickups;
    private static int livePickupCount = 0;
    void Start()
    {
        livePickupCount = 0;
        maxPickups = basePickups + addPickupsPerPlayers * (GameInfo.playerIndices.Count - 1);

        for (int i = 0; i < GameInfo.playerIndices.Count; i++)
        {
            SpawnNewPickup(new Vector3(players[i].transform.position.x, spawnCenter.position.y, players[i].transform.position.z), players[i].transform.forward * 2.5f);
            livePickupCount++;
        }
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

    /* from is start position, to is displacement */
    private GameObject SpawnNewPickup(Vector3 from, Vector3 to)
    {
        return Instantiate(pickupPrefab, from + to, transform.rotation);
    }

    public static void DecrementPickup()
    {
        livePickupCount--;
    }
}
