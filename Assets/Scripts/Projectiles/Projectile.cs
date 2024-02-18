using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private int whoShot;
    private float startDistance;
    [SerializeField] private float delayDestroyTime;
    [SerializeField] private float reactivateTime;

    private bool isDoubled;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        transform.GetChild(0).gameObject.tag = "Untagged";
        StartCoroutine(ReactivateAfterTime(reactivateTime));
    }

    private void Update()
    {
        if (rb.velocity.y == 0)
        {
            rb.velocity *= .98f;
            rb.angularVelocity *= .98f;
        }
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

    public bool GetIsDoublePoints()
    {
        return isDoubled;
    }

    public float GetStartDistance()
    {
        return startDistance;
    }

    public IEnumerator ReactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        transform.GetChild(0).gameObject.tag = "Egg";
    }

    public void SetNextShotDoubled(bool b)
    {
        isDoubled = b;
    }
}
