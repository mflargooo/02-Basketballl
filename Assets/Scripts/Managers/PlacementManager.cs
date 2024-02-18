using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject[] players;
    [SerializeField] private GameObject[] podiums;
    [SerializeField] private Transform[] scoreTexts;
    [SerializeField] private Transform lookAt;

    // Start is called before the first frame update
    void Start()
    {
        AssignPodiums();
    }

    public void AssignPodiums()
    {
        int numPlaces = GameInfo.placementsLastToFirst.Count;
        int playersSet = 0;

        for (int i = 0; i < numPlaces; i++)
        {
            List<int> sortedPIDs = GameInfo.placementsLastToFirst.Values[i];
            IList<int> scores = GameInfo.placementsLastToFirst.Keys;

            float podiumWidth = podiums[i].transform.localScale.z;
            float delW = podiumWidth / (sortedPIDs.Count + 1);

            playersSet += sortedPIDs.Count;
            for (int j = 0; j < sortedPIDs.Count; j++)
            {
                int pid = sortedPIDs[j];
                GameObject model = players[pid].transform.GetChild(GameInfo.characterSelectIndexes[pid]).gameObject;
                model.SetActive(true);

                int placement = GameManager.GetNumPlayers() - playersSet;
                if (placement == 0) model.GetComponent<Animator>().Play("1");
                else if (placement == 1)  model.GetComponent<Animator>().Play("2");
                else if (placement == 2) model.GetComponent<Animator>().Play("3");
                else model.GetComponent<Animator>().Play("4");

                scoreTexts[numPlaces - i + 1].GetChild(0).GetComponent<TMP_Text>().text = scores[numPlaces - i - 1].ToString();
                scoreTexts[numPlaces - i + 1].gameObject.SetActive(true);

                players[pid].transform.position = new Vector3(podiums[numPlaces - i - 1].transform.position.x + delW * (j + 1) - podiumWidth / 2f, podiums[numPlaces - i - 1].transform.localScale.y, podiums[numPlaces - i - 1].transform.position.z);
                lookAt.position = lookAt.position + Vector3.up * (players[pid].transform.position.y - lookAt.position.y);
                players[pid].transform.LookAt(lookAt);

            }
        }
    }
}
