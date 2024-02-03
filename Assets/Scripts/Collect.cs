using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    [SerializeField] private PlayerStats ps;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Egg" && ps.eggCt < GameManager.GetMaxEggCount())
        {
            UIManager.UpdateEggs(ps.id, ++ps.eggCt);
            /* collect egg sound */
            Destroy(other.gameObject);
        }
        else if(other.gameObject.tag == "Powerup")
        {
            /*powerup pickup sound*/
            ps.powerupID = -1 /*need powerup id system*/;
        }
    }
}
