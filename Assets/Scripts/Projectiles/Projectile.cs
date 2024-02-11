using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private int whoShot;
    private float startDistance;
    [SerializeField] private float delayDestroyTime;
    [SerializeField] private float disappearTime;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Disappear(delayDestroyTime, disappearTime));
    }

    public void LaunchAt(int pid, Vector3 target, float launchStrength)
    {
        whoShot = pid;
        Vector3 displacement = (target - transform.position);
        Vector3 horzDisplacement = new Vector3(displacement.x, 0f, displacement.z);

        startDistance = horzDisplacement.magnitude;

        float horizontalSpeed = startDistance / launchStrength;

        float airTime = horzDisplacement.magnitude / horizontalSpeed;
        float verticalSpeed = displacement.y / airTime - .5f * Physics.gravity.y * airTime;

        rb.velocity = horzDisplacement.normalized * horizontalSpeed + Vector3.up * verticalSpeed;
    }

    public void LaunchTo(int pid, Vector3 launchDir, float launchSpeed)
    {
        whoShot = pid;
        rb.velocity = launchDir.normalized * launchSpeed;
    }

    public int GetWhoShot()
    {
        return whoShot;
    }

    public float GetStartDistance()
    {
        return startDistance;
    }

    public IEnumerator Disappear(float delay, float time)
    {
        yield return new WaitForSeconds(delay);
        float startTime = time;
        Vector3 startScale = transform.localScale;
        while(time > 0)
        {
            time -= Time.deltaTime;
            transform.localScale = startScale * (time / startTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SpawnManager.DecrementPickup();   
    }
}
