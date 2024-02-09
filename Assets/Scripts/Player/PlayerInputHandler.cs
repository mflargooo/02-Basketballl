using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput pi;
    private PlayerController pc;
    private Shooting sh;

    public void Setup(GameManager gm)
    {
        pi = GetComponent<PlayerInput>();

        GameObject currPlayer = gm.GetPlayers()[pi.playerIndex];
        currPlayer.SetActive(true);
        pc = currPlayer.GetComponent<PlayerController>();
        sh = currPlayer.GetComponent<Shooting>();

        UIManager.EnablePlayer(pi.playerIndex);
    }
    public void OnMove(CallbackContext context)
    {
        if (pc)
            pc.SetInputVector(context.ReadValue<Vector2>());
    }

    public void OnShoot(CallbackContext context)
    {
        if (sh)
            sh.SetAttemptingShot(context.ReadValue<float>());

    }

    public void OnDash(CallbackContext context)
    {
        if (pc)
            pc.SetDashPressed(context.ReadValue<float>());
    }

    public void OnDeviceDisconnect()
    {
        UIManager.DisablePlayer(pi.playerIndex);
        pc.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
