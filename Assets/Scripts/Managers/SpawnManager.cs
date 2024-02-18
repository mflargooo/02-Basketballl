using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Balls")]
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private int basePickups = 3;
    [SerializeField] private int addPickupsPerPlayers = 2;

    [SerializeField] private GameObject[] players;

    private int maxPickups;
    private static int livePickupCount = 0;

    [Header("Powerups")]
    [SerializeField] private GameObject[] doublePtsPUPrefab = new GameObject[4];
    [SerializeField] private float doubleSpawnCooldown = 10f;
    private GameObject spawnedDoublePts;

    void Start()
    {
        livePickupCount = 0;
        maxPickups = basePickups + addPickupsPerPlayers * (GameInfo.playerIndices.Count - 1);

        for (int i = 0; i < GameInfo.playerIndices.Count; i++)
        {
            SpawnNewPickup(new Vector3(players[i].transform.position.x, spawnCenter.position.y, players[i].transform.position.z), players[i].transform.forward * 6f);
            livePickupCount++;
        }

        StartCoroutine(SpawnPowerups());
    }
    // Update is called once per frame
    void Update()
    {
        if (livePickupCount < maxPickups)
        {
            RandomObjectSpawn(pickupPrefab, spawnCenter.transform.position);
            livePickupCount++;
        }
    }

    private GameObject RandomObjectSpawn(GameObject obj, Vector3 center)
    {
        float angle = Random.Range(0f, 360f);
        float dist = Random.Range(minRadius, maxRadius);
        Vector3 spawnPos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * dist + center;

        GameObject instance = Instantiate(obj, spawnPos, transform.rotation);
        if (instance.name.Contains("Ball")) instance.GetComponent<Animator>().Play("BallSpawn");
        return instance;
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

    IEnumerator SpawnPowerups()
    {
        while (true)
        {
            if (spawnedDoublePts) yield return null;
            else
            {
                yield return new WaitForSeconds(doubleSpawnCooldown);
                int index = Random.Range(0, doublePtsPUPrefab.Length);
                spawnedDoublePts = RandomObjectSpawn(doublePtsPUPrefab[index], spawnCenter.transform.position);
            }
        }
    }
}
