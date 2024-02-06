using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    private PlayerController pc;
    [SerializeField] private float dashToStunAngle;
    private void Start()
    {
        pc = transform.root.GetComponent<PlayerController>();
    }
    private void OnTriggerStay(Collider other)
    {
        Vector3 toTarget = other.transform.position - transform.position;
        if(pc.IsDashing() && Vector3.Angle(toTarget, transform.forward) < dashToStunAngle  && other.gameObject.tag == "Player")
        {
            StartCoroutine(other.gameObject.GetComponent<PlayerEffects>().Stun(transform.forward));
        }
    }
}
