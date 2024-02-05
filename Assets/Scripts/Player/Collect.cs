using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    [SerializeField] private int playerID;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Egg" && GameManager.ps[playerID].eggCt < GameManager.GetMaxEggCount())
        {
            UIManager.UpdateEggs(GameManager.ps[playerID].id, ++GameManager.ps[playerID].eggCt);
            /* collect egg sound */
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Powerup")
        {
            /*powerup pickup sound*/
            GameManager.ps[playerID].powerupID = -1 /*need powerup id system*/;
        }
    }
}
