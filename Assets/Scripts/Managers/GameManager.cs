using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float gameTime = 60;
    [SerializeField] private int maxCarryEggs;
    private static int mcEggs;

    [SerializeField] private GameObject[] players;
    [SerializeField] private PlayerStats[] playerStats;

    public static PlayerStats[] ps;
    [SerializeField] private Color[] playerColors;
    public static Color[] pc;

    private float gameTimer;
    bool endGame;

    public static CharacterSoundEffects[] charSFX;
    [SerializeField] private AudioClip[] announcerStartSounds;
    [SerializeField] private AudioClip tenSecsLeft;
    [SerializeField] private GameObject clock;
    bool playTenSecsLeft;

    bool started;

    // Start is called before the first frame update
    void Start()
    {
        started = false;
        charSFX = new CharacterSoundEffects[4];
        gameTime += .9999f;
        mcEggs = maxCarryEggs;
        ps = playerStats;
        pc = playerColors;

        StartCoroutine(BeginGame());
    }

    private void Update()
    {
        if (!started || endGame) return;

        if(gameTimer <= 10.5f && !playTenSecsLeft)
        {
            playTenSecsLeft = true;
            Camera.main.GetComponent<AudioSource>().PlayOneShot(tenSecsLeft);
        }
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
        playTenSecsLeft = false;
        endGame = false;
        gameTimer = gameTime;
        ResetPlayers();
        UIManager.SetupUI(indices);
    }
    private static void ResetPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            ps[i].placement = 1;
            ps[i].score = 0;
            ps[i].eggCt = 0;
            ps[i].powerupID = -1;
        }
    }

    public static int GetNumPlayers()
    {
        return GameInfo.playerIndices.Count;
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
        yield return new WaitForSeconds(1f);
        /*load win screen and track placements*/
        NewGame(GameInfo.playerIndices);
    }

    IEnumerator BeginGame()
    {
        for (int i = 0; i < 4; i++)
        {
            if(GameInfo.playerInputObjs[3 - i])
                GameInfo.playerInputObjs[3 - i].SetActive(false);
        }

        AudioClip selected = null;
        if (Random.Range(0f, 1f) < .2)
            selected = announcerStartSounds[0];
        else selected = announcerStartSounds[1];

        Camera.main.GetComponent<AudioSource>().PlayOneShot(selected);

        yield return new WaitForSeconds(selected.length - .75f);
        
        NewGame(GameInfo.playerIndices);
        clock.SetActive(true);
        foreach (int i in GameInfo.playerIndices)
        {
            GameObject model = players[i].transform.GetChild(1).GetChild(GameInfo.characterSelectIndexes[i]).gameObject;
            model.SetActive(true);
            players[i].GetComponent<PlayerController>().SetAnimator(model.GetComponent<Animator>());
            players[i].GetComponent<Shooting>().SetAnimator(model.GetComponent<Animator>());
            players[i].GetComponent<PlayerEffects>().SetAnimator(model.GetComponent<Animator>());
            charSFX[i] = model.GetComponent<CharacterSoundEffects>();
        }

        started = true;

        yield return null;
        for (int i = 0; i < 4; i++)
        {
            if (GameInfo.playerInputObjs[3 - i])
            {
                GameInfo.playerInputObjs[3 - i].SetActive(true);
                GameInfo.playerInputObjs[3 - i].GetComponent<PlayerInputHandler>().Setup(this);
            }
        }

        yield return null;
        for (int i = 0; i < 4; i++)
        {
            if (GameInfo.playerInputObjs[3 - i])
            {
                GameInfo.playerInputObjs[3 - i].SetActive(false);
            }
        }

        yield return null;
        for (int i = 0; i < 4; i++)
        {
            if (GameInfo.playerInputObjs[3 - i])
            {
                GameInfo.playerInputObjs[3 - i].SetActive(true);
            }
        }
    }
}

