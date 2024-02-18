using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

public class CharacterSelectInputHandler : MonoBehaviour
{
    private PlayerInput pi;
    private SelectScreenManager ssm;

    private bool hasIncred;
    private bool hasDecred;

    private bool canReady;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        pi = GetComponent<PlayerInput>();
        ssm = FindObjectOfType<SelectScreenManager>();
        ssm.AddPlayer(pi.playerIndex);
        GameInfo.playerInputObjs[pi.playerIndex] = gameObject;

        StartCoroutine(ReadyCooldown());
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1) ssm = FindObjectOfType<SelectScreenManager>();
    }

    public void IncrCharacterIndex(CallbackContext context)
    {
        if (ssm && !hasIncred && context.ReadValueAsButton())
        {
            ssm.SetCharacterIndex(pi.playerIndex, 1);
            hasIncred = true;
        }
        if (!context.ReadValueAsButton()) hasIncred = false;
    }
    public void DecrCharacterIndex(CallbackContext context)
    {
        if (ssm && !hasDecred && context.ReadValueAsButton())
        {
            ssm.SetCharacterIndex(pi.playerIndex, -1);
            hasDecred = true;
        }
        if (!context.ReadValueAsButton()) hasDecred = false;
    }

    public void Ready(CallbackContext context)
    {
        if(ssm && context.ReadValue<float>() > 0 && canReady)
            ssm.Ready(pi.playerIndex);
    }
    
    public void Unready(CallbackContext context)
    {
        if (ssm && context.ReadValue<float>() > 0)
            ssm.Unready(pi.playerIndex);
    }

    public void OnDeviceDisconnect()
    {
        if (!ssm || SceneManager.GetActiveScene().buildIndex != 1) return;

        ssm.Unready(pi.playerIndex);
        ssm.RemovePlayer(pi.playerIndex);
        GameInfo.playerInputObjs[pi.playerIndex] = null;
    }

    public void OnDeviceRegain()
    {
        if (!ssm || SceneManager.GetActiveScene().buildIndex != 1) return;

        ssm.Ready(pi.playerIndex);
        ssm.AddPlayer(pi.playerIndex);
        GameInfo.playerInputObjs[pi.playerIndex] = gameObject;
    }

    IEnumerator ReadyCooldown()
    {
        yield return null;
        canReady = true;
    }
}