using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    private PlayerController pc;
    private Shooting sh;

    [SerializeField] private float dashToStunAngle;
    [SerializeField] private AudioSource quieterAS;
    [SerializeField] private AudioClip bballPickupClip;
    [SerializeField] private AudioClip pUpPickupClip;
    private void Start()
    {
        pc = transform.root.GetComponent<PlayerController>();
        sh = transform.root.GetComponent<Shooting>();
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
        else if (other.gameObject.tag == "Egg" && GameManager.ps[pc.GetPlayerID()].GetComponent<PlayerStats>().eggCt < GameManager.GetMaxEggCount())
        {
            Destroy(other.transform.root.gameObject);
            quieterAS.PlayOneShot(bballPickupClip);
            GameManager.ps[pc.GetPlayerID()].eggCt++;
            UIManager.UpdateEggs(pc.GetPlayerID(), GameManager.ps[pc.GetPlayerID()].eggCt);
        }
        else if (other.gameObject.tag == "Powerup" && !sh.GetNextShotDoubled())
        {
            Destroy(other.gameObject);
            quieterAS.PlayOneShot(pUpPickupClip);
            sh.SetNextShotDoubled(true);
        }
    }


}
