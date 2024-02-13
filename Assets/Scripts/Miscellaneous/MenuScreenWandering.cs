using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenWandering : MonoBehaviour
{
    [SerializeField] private GameObject wandererPrefab;
    [SerializeField] private uint numWanderers;
    [SerializeField] private float wanderSpeed;
    [SerializeField] private float rotateSpeed;
    private Vector3 center;
    [SerializeField] private Vector2 wanderRange;
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        for(int i = 0; i < numWanderers; i++)
        {
            Vector3 xOffset = Vector3.right * (Random.Range(0f, 1f) < .5 ? 1 : -1) * (Random.Range(wanderRange.x, wanderRange.x + 2f));
            Vector3 zOffset = Vector3.forward * (Random.Range(0f, 1f) < .5 ? 1 : -1) * (Random.Range(wanderRange.y, wanderRange.y + 3f));
            GameObject wanderer = Instantiate(wandererPrefab, center + xOffset + zOffset, transform.rotation);
            StartCoroutine(Wander(wanderer));
        }
    }

    Vector3 ChooseNewWanderPoint()
    {
        return center + new Vector3(Random.Range(-wanderRange.x / 2, wanderRange.x / 2), 0f, Random.Range(-wanderRange.y / 2, wanderRange.y / 2));
    }

    IEnumerator Wander(GameObject obj)
    {
        Vector3 target = obj.transform.position;

        while(true)
        {
            if ((target - obj.transform.position).magnitude <= .1f)
            {
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
                target = ChooseNewWanderPoint();
            }
            else
            {
                Vector3 dir = (target - obj.transform.position).normalized;
                float angle = Vector3.SignedAngle(obj.transform.forward, dir, transform.up);
                obj.transform.Rotate(Vector3.up * angle * Time.deltaTime * rotateSpeed);
                obj.transform.Translate(transform.forward * wanderSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(wanderRange.x, 1f, wanderRange.y));
    }
}
