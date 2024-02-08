using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealing : MonoBehaviour
{
    public void stealing(GameObject player)
    {
        int playerID = player.GetComponent<PlayerController>().GetPlayerID();
        int thisPlayerID = transform.root.GetComponent<PlayerController>().GetPlayerID();
        bool stealerHasSpace = GameManager.ps[playerID].GetComponent<PlayerStats>().eggCt < GameManager.GetMaxEggCount();
        bool stealeeHasEggs = GameManager.ps[thisPlayerID].GetComponent<PlayerStats>().eggCt > 0;

        if (stealeeHasEggs && stealerHasSpace)
        {
            GameManager.ps[playerID].GetComponent<PlayerStats>().eggCt++;
            UIManager.UpdateEggs(playerID, GameManager.ps[playerID].GetComponent<PlayerStats>().eggCt);

            GameManager.ps[thisPlayerID].GetComponent<PlayerStats>().eggCt--;
            UIManager.UpdateEggs(thisPlayerID, GameManager.ps[thisPlayerID].GetComponent<PlayerStats>().eggCt);
        }
        //animation?
    }
}

