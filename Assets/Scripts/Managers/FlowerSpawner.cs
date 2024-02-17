using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 flowerSpawnRange = new Vector2(30f, 30f);
    [SerializeField] private float flowerMinScale;
    [SerializeField] private float flowerMaxScale;
    [SerializeField] private int flowerRows = 10;
    [SerializeField] private int flowerCols = 10;
    [SerializeField] private float cellSpawnChance = .5f;
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private Material[] flowerMaterials;
    // Start is called before the first frame update
    void Start()
    {
        float delRow = flowerSpawnRange.x / flowerRows;
        float delCol = flowerSpawnRange.y / flowerCols;
        Vector3 origin = transform.position - new Vector3(flowerSpawnRange.x * .5f, transform.position.y, flowerSpawnRange.y * .5f);

        for (int x = 0; x < flowerRows; x++)
        {
            for (int z = 0; z < flowerCols; z++)
            {
                if (Random.Range(0f, 1f) > cellSpawnChance) continue;

                Vector3 randCellPos = new Vector3(Random.Range(.5f + delRow * x, delRow * (x + 1) - .5f), .1f, Random.Range(.5f + delCol * z, delCol * (z + 1) - .5f));
                GameObject flower = Instantiate(flowerPrefab, origin + randCellPos, Quaternion.Euler(90f, 0f, Random.Range(0f, 360f)));
                flower.GetComponent<MeshRenderer>().material = flowerMaterials[Random.Range(0, flowerMaterials.Length)];
                float scale = Random.Range(flowerMinScale, flowerMaxScale);
                flower.transform.localScale = new Vector3(scale, scale, 1f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(30f, 1f, 30f));
    }
}
