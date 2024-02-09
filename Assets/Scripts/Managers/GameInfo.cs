using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public static List<int> playerIndices = new List<int>();
    public static int[] characterSelectIndexes;
    public static PlayerInput[] pis = new PlayerInput[4];
    [SerializeField] private InputActionAsset playerControls;
    private static InputActionAsset pc;

    public static GameInfo instance {get ; private set; }
    public void Awake()
    {
        pc = playerControls;
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

    public static void SetUpPlayers()
    {
        foreach(int idx in playerIndices)
        {
            pis[idx].actions = pc;
            pis[idx].defaultActionMap = "Player";

            if (pis[idx].GetComponent<CharacterSelectInputHandler>())
                Destroy(pis[idx].GetComponent<CharacterSelectInputHandler>());

            if (!pis[idx].GetComponent<PlayerInputHandler>())
                pis[idx].gameObject.AddComponent<PlayerInputHandler>();
        }
    }
}
