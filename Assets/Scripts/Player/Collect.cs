using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private int playerID;
    private void Start()
    {
        playerID = GetComponent<PlayerController>().GetPlayerID();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Egg" && GameManager.ps[playerID].GetComponent<PlayerStats>().eggCt < GameManager.GetMaxEggCount())
        {
            Destroy(other.gameObject);
            GameManager.ps[playerID].eggCt++; 
            UIManager.UpdateEggs(playerID, GameManager.ps[playerID].eggCt);
            /* collect egg sound */
        }
        else if (other.gameObject.tag == "Powerup")
        {
            /*powerup pickup sound*/
            //GameManager.ps[playerID].GetComponent<PlayerStats>().powerupID = -1 /*need powerup id system*/;

            Destroy(other.gameObject);
        }
    }
}
