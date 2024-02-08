using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealing : MonoBehaviour
{
    public void stealing(GameObject player)
    {
        int playerID = player.GetComponent<PlayerController>().GetPlayerID();
        if (GameManager.ps[playerID] && GameManager.ps[playerID].GetComponent<PlayerStats>().eggCt < GameManager.GetMaxEggCount())
        {
            GameManager.ps[playerID].GetComponent<PlayerStats>().eggCt++;
            UIManager.UpdateEggs(GameManager.ps[playerID].id, GameManager.ps[playerID].GetComponent<PlayerStats>().eggCt);
        }

        int thisPlayerID = transform.root.GetComponent<PlayerController>().GetPlayerID();
        if (GameManager.ps[thisPlayerID] && GameManager.ps[thisPlayerID].GetComponent<PlayerStats>().eggCt > 0)
        {
            GameManager.ps[thisPlayerID].GetComponent<PlayerStats>().eggCt--;
            UIManager.UpdateEggs(GameManager.ps[thisPlayerID].id, GameManager.ps[thisPlayerID].GetComponent<PlayerStats>().eggCt);
        }
        //animation?
    }
}

