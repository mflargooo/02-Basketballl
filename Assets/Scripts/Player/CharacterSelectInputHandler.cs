using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

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

        GameInfo.pis[pi.playerIndex] = pi;
    }

    public void IncrCharacterIndex(CallbackContext context)
    {
        ssm.SetCharacterIndex(pi.playerIndex, context.ReadValueAsButton() ? 1 : 0);
    }
    public void DecrCharacterIndex(CallbackContext context)
    {
        ssm.SetCharacterIndex(pi.playerIndex, context.ReadValueAsButton() ? -1 : 0);
    }

    public void Ready(CallbackContext context)
    {
        if(context.ReadValue<float>() > 0)
            ssm.Ready(pi.playerIndex);
    }
    
    public void Unready(CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
            ssm.Unready(pi.playerIndex);
    }

    public void OnDeviceDisconnect()
    {
        ssm.Unready(pi.playerIndex);
        ssm.RemovePlayer(pi.playerIndex);
        Destroy(gameObject);
    }
}