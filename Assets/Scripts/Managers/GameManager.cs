using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float gameTime = 60;
    [SerializeField] private int maxCarryEggs;
    private static int numPlayers = 4;
    private static int mcEggs;

    [SerializeField] private GameObject[] players;
    [SerializeField] private PlayerStats[] playerStats;

    public static PlayerStats[] ps;
    [SerializeField] private Color[] playerColors;
    public static Color[] pc;

    private float gameTimer;
    bool endGame;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(ResetPlayerInputs());

        gameTime += .9999f;
        mcEggs = maxCarryEggs;
        ps = playerStats;
        pc = playerColors;
        NewGame(GameInfo.playerIndices.Count);
    }

    private void Update()
    {
        if (gameTimer < 1f && !endGame)
        {
            endGame = true;
            StartCoroutine(EndGame());
        }
        else
        {
            gameTimer -= Time.deltaTime;
        }

        if (gameTimer > 0f)
        {
            UIManager.UpdateClock(gameTimer);
        }
    }

    public void NewGame(int playerCount)
    {
        endGame = false;
        gameTimer = gameTime;
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

    public static int GetNumPlayers()
    {
        return numPlayers;
    }

    public static int GetMaxEggCount()
    {
        return mcEggs;
    }

    public GameObject[] GetPlayers()
    {
        return players;
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(4f);
        NewGame(numPlayers);
    }

    IEnumerator ResetPlayerInputs()
    {
        PlayerInputHandler[] pihs = FindObjectsOfType<PlayerInputHandler>();
        foreach (PlayerInputHandler pih in pihs)
        {
            pih.Setup(this);
            pih.gameObject.SetActive(false);
        }
        yield return null;
        foreach (PlayerInputHandler pih in pihs)
        {
            pih.gameObject.SetActive(true);
        }
    }
}
