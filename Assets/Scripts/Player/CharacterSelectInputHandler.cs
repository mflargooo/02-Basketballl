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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        pi = GetComponent<PlayerInput>();
        ssm = FindObjectOfType<SelectScreenManager>();
        ssm.AddPlayer(pi.playerIndex);
        GameInfo.playerInputObjs[pi.playerIndex] = gameObject;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1) ssm = FindObjectOfType<SelectScreenManager>();
    }

    public void IncrCharacterIndex(CallbackContext context)
    {
        if(ssm)
            ssm.SetCharacterIndex(pi.playerIndex, context.ReadValueAsButton() ? 1 : 0);
    }
    public void DecrCharacterIndex(CallbackContext context)
    {
        if (ssm)
            ssm.SetCharacterIndex(pi.playerIndex, context.ReadValueAsButton() ? -1 : 0);
    }

    public void Ready(CallbackContext context)
    {
        if(ssm && context.ReadValue<float>() > 0)
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
}