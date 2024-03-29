using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public static List<int> playerIndices = new List<int>();
    public static int[] characterSelectIndexes = new int[4];

    public static GameObject[] playerInputObjs = new GameObject[4];

    public static SortedList<int, List<int>> placementsLastToFirst;

    public static bool isRematch;

    public static GameInfo instance {get ; private set; }
    public void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
}
