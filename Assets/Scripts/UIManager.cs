using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerIcons;
    [SerializeField] private Color[] placementColors;

    private static GameObject[] pi;
    private static Color[] pc;

    private void Start()
    {
        pi = playerIcons;
        pc = placementColors;
    }

    public static void SetupUI()
    {
        int numPlayers = GameManager.GetNumPlayers();

        int width = Screen.width;
        int deltaW = width / (numPlayers + 1);
        for (int i = 0; i < numPlayers; i++)
        {
            RectTransform icon = pi[i].GetComponent<RectTransform>();
            icon.position = new Vector3(deltaW * (i + 1), icon.position.y, icon.position.z);
            icon.gameObject.SetActive(true);
        }

        for (int i = 0; i < numPlayers; i++)
        {
            /*reset egg icons*/
            UpdateEggs(i, 0);
            /* update icons */
            //...
            /* reset placements */
            TMP_Text placementText = pi[i].transform.GetChild(2).GetComponent<TMP_Text>();
            placementText.text = numPlayers.ToString();
            placementText.color = pc[numPlayers - 1];


            /* clear powerup */
            pi[i].transform.GetChild(3).GetComponent<Image>().sprite = null /* replace w/ empty image */;

            /* reset score */
            pi[i].transform.GetChild(4).GetComponent<TMP_Text>().text = "Score: 0";

        }
    }

    public static void UpdateEggs(int pid, int numEggs)
    {
        print(pid.ToString() + ", " + numEggs.ToString());
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
}
