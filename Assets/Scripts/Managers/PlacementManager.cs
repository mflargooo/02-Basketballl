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

        for (int i = numPlaces - 1; i >= 0; i--)
        {
            float podiumWidth = podiums[i].transform.localScale.z;
            List<int> pids = GameInfo.placementsLastToFirst[i];
            float delW = podiumWidth / (pids.Count + 1);
            for (int j = 0; j < pids.Count; j++)
            {
                players[pids[j]].transform.position = new Vector3(podiums[i].transform.position.x + delW * (j + 1) - podiumWidth / 2f, podiums[i].transform.localScale.y, podiums[i].transform.position.z);
                lookAt.position = lookAt.position + Vector3.up * (players[pids[j]].transform.position.y - lookAt.position.y);
                players[pids[j]].transform.LookAt(lookAt);
            }

        }
    }
}
