using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxCarryEggs;
    private static int numPlayers = 2;
    private static int mcEggs;

    [SerializeField] private PlayerStats[] playerStats;

    private static PlayerStats[] ps;

    // Start is called before the first frame update
    void Start()
    {
        mcEggs = maxCarryEggs;
        ps = playerStats;
        NewGame(numPlayers); /* this will get removed when we have a menu that starts the game */
    }

    public static void NewGame(int playerCount)
    {
        numPlayers = playerCount;
        ResetPlayers();
        UIManager.SetupUI();
    }
    private static void ResetPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            ps[i].placement = numPlayers;
            ps[i].score = 0;
            ps[i].eggCt = 0;
            ps[i].powerupID = -1;
        }
    }

    public static void SetNumPlayers(int players)
    {
        numPlayers = players;
    }

    public static int GetNumPlayers()
    {
        return numPlayers;
    }

    public static int GetMaxEggCount()
    {
        return mcEggs;
    }
}
