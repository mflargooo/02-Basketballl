using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxCarryEggs;
    private static int numPlayers = 4;
    private static int mcEggs;

    [SerializeField] private PlayerStats[] playerStats;

    public static PlayerStats[] ps;
    [SerializeField] private Color[] playerColors;
    public static Color[] pc;

    // Start is called before the first frame update
    void Start()
    {
        mcEggs = maxCarryEggs;
        ps = playerStats;
        pc = playerColors;
        NewGame(numPlayers); /* this will get removed when we have a menu that starts the game -- maybe */
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
