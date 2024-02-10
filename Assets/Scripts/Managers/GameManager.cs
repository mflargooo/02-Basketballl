using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        foreach(int i in GameInfo.playerIndices)
        {
            GameObject model = players[i].transform.GetChild(1).GetChild(GameInfo.characterSelectIndexes[i]).gameObject;
            model.SetActive(true);
            players[i].GetComponent<PlayerController>().SetAnimator(model.GetComponent<Animator>());
            players[i].GetComponent<Shooting>().SetAnimator(model.GetComponent<Animator>());
            players[i].GetComponent<PlayerEffects>().SetAnimator(model.GetComponent<Animator>());
        }

        NewGame(GameInfo.playerIndices);
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

    public void NewGame(List<int> indices)
    {
        endGame = false;
        gameTimer = gameTime;
        numPlayers = indices.Count;
        ResetPlayers();
        UIManager.SetupUI(indices);
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
        NewGame(GameInfo.playerIndices);
    }

    IEnumerator ResetPlayerInputs()
    {
        PlayerInputHandler[] pihs = FindObjectsOfType<PlayerInputHandler>();
        for (int i = pihs.Length - 1; i >= 0; i--)
        {
            pihs[i].Setup(this);
            pihs[i].gameObject.SetActive(false);
        }
        yield return null;
        for (int i = pihs.Length - 1; i >= 0; i--)
        {
            pihs[i].gameObject.SetActive(true);
        }
    }
}
