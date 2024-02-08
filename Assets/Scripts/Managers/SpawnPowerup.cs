using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerup : MonoBehaviour
{
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private float spawnHeight = 50f;
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private int maxPickups;
    private GameObject[] spawned;
    // Start is called before the first frame update
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
        Vector3 spawnPos = spawnCenter.position + new Vector3(Mathf.Cos(angle) * dist, spawnHeight, Mathf.Sin(angle) * dist);

        GameObject newPickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);
        StartCoroutine(FallToGround(newPickup));

        return newPickup;
    }

    private IEnumerator FallToGround(GameObject pickup)
    {
        while (pickup != null && pickup.transform.position.y > 0.5f)
        {
            pickup.transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        if (pickup != null)
        {
            Destroy(pickup);
        }
    }
}
