using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    private PlayerController pc;

    [SerializeField] private float dashToStunAngle;
    [SerializeField] private AudioSource quieterAS;
    [SerializeField] private AudioClip bballPickupClip;
    private void Start()
    {
        pc = transform.root.GetComponent<PlayerController>();
    }
    private void OnTriggerStay(Collider other)
    {
        Vector3 toTarget = other.transform.position - transform.position;
        if (pc.IsDashing() && Vector3.Angle(toTarget, transform.forward) < dashToStunAngle && other.gameObject.tag == "Player")
        {
            if (!other.GetComponent<PlayerEffects>().invuln)
            {
                StartCoroutine(other.gameObject.GetComponent<PlayerEffects>().Stun(transform));
                other.gameObject.GetComponent<Stealing>().stealing(transform.root.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Egg" && GameManager.ps[pc.GetPlayerID()].GetComponent<PlayerStats>().eggCt < GameManager.GetMaxEggCount())
        {
            Destroy(other.gameObject);
            quieterAS.PlayOneShot(bballPickupClip);
            GameManager.ps[pc.GetPlayerID()].eggCt++;
            UIManager.UpdateEggs(pc.GetPlayerID(), GameManager.ps[pc.GetPlayerID()].eggCt);
        }
    }


}
