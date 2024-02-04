using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private int whoShot;
    private float startDistance;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void LaunchAt(int pid, GameObject target, float launchStrength)
    {
        whoShot = pid;
        Vector3 displacement = (target.transform.position - transform.position);
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
}
