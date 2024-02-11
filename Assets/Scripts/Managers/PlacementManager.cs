using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject[] players;
    [SerializeField] private GameObject[] podiums;
    [SerializeField] private Transform lookAt;

    // Start is called before the first frame update
    void Start()
    {
        AssignModels();
        AssignPodiums();
    }

    public void AssignModels()
    {
        foreach(int pid in GameInfo.playerIndices)
        {
            players[pid].transform.GetChild(GameInfo.characterSelectIndexes[pid]).gameObject.SetActive(true);
        }
    }

    public void AssignPodiums()
    {
        int numPlaces = GameInfo.placementsLastToFirst.Count;
        int playersSet = 0;
        for (int i = 0; i < numPlaces; i++)
        {
            List<int> sortedPIDs = GameInfo.placementsLastToFirst.Values[i];

            float podiumWidth = podiums[i].transform.localScale.z;
            float delW = podiumWidth / (sortedPIDs.Count + 1);

                playersSet += sortedPIDs.Count;
            for (int j = 0; j < sortedPIDs.Count; j++)
            {
                int placement = GameManager.GetNumPlayers() - playersSet;
                players[sortedPIDs[j]].transform.position = new Vector3(podiums[numPlaces - i - 1].transform.position.x + delW * (j + 1) - podiumWidth / 2f, podiums[numPlaces - i - 1].transform.localScale.y, podiums[numPlaces - i - 1].transform.position.z);
                lookAt.position = lookAt.position + Vector3.up * (players[sortedPIDs[j]].transform.position.y - lookAt.position.y);
                players[sortedPIDs[j]].transform.LookAt(lookAt);

            }
        }
    }
}
