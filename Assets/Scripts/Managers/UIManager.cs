using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerIcons;
    [SerializeField] private Color[] placementColors;

    [SerializeField] private TMP_Text timerText;

    private static GameObject[] pi;
    private static Color[] pc;
    private static TMP_Text tt;

    private void Awake()
    {
        pi = playerIcons;
        pc = placementColors;
        tt = timerText;

        foreach (GameObject p in pi) p.SetActive(false);
    }

    public static void SetupUI()
    {
        /*int width = Screen.width;
        int deltaW = width / (numPlayers + 1);
        for (int i = 0; i < numPlayers; i++)
        {
            RectTransform icon = pi[i].GetComponent<RectTransform>();
            icon.position = new Vector3(deltaW * (i + 1), icon.position.y, icon.position.z);
            icon.gameObject.SetActive(true);
        }*/

        for (int i = 0; i < 4; i++)
        {
            /*reset egg icons*/
            UpdateEggs(i, 0);
            /* update icons */
            //...
            /* reset placements */
            /*TMP_Text placementText = playerIcons[i].transform.GetChild(2).GetComponent<TMP_Text>();
            placementText.text = numPlayers.ToString();
            placementText.color = placementColors[numPlayers - 1]; */


            /* clear powerup */
            pi[i].transform.GetChild(3).GetComponent<Image>().sprite = null /* replace w/ empty image */;

            /* reset score */
            UpdateScore(i, 0);
        }
    }

    public static void UpdateEggs(int pid, int numEggs)
    {
        Transform eggs = pi[pid].transform.GetChild(0);
        for (int i = 0; i < numEggs; i++)
        {
            eggs.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = numEggs; i < GameManager.GetMaxEggCount(); i++)
        {
            eggs.GetChild(i).gameObject.SetActive(false);
        }
    }

    public static void UpdateScore(int pid, int score)
    {
        pi[pid].transform.GetChild(4).GetComponent<TMP_Text>().text = "Score: " + score.ToString();
        UpdatePlacements();
    }

    private static void UpdatePlacements()
    {
        SortedList<int, List<int>> scoreID = new SortedList<int, List<int>>();

        for (int i = 0; i < GameManager.GetNumPlayers(); i++)
        {
            int score = GameManager.ps[i].score;
            if (!scoreID.ContainsKey(score))
                scoreID.Add(score, new List<int>());
            scoreID[score].Add(i);
        }

        int playersSet = 0;
        for (int i = 0; i < scoreID.Count; i++)
        {
            List<int> sortedPIDs = scoreID.Values[i];
            playersSet += sortedPIDs.Count;
            foreach (int spid in sortedPIDs)
            {
                TMP_Text placementText = pi[spid].transform.GetChild(2).GetComponent<TMP_Text>();
                placementText.text = (GameManager.GetNumPlayers() - playersSet + 1).ToString();
                placementText.color = pc[GameManager.GetNumPlayers() - playersSet];
            }
        }
    }

    public static void UpdateClock(float time)
    {
        tt.text = ((int) time).ToString();
    }

    public static void EnablePlayer(int pid)
    {
        pi[pid].SetActive(true);
    }
}