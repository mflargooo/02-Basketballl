using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput pi;
    private PlayerController pc;
    private Shooting sh;

    public void Setup(GameManager gm)
    {
        pi = GetComponent<PlayerInput>();
        pi.defaultActionMap = "Player";
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
        if (SceneManager.GetActiveScene().buildIndex != 2) return;

        GameInfo.playerIndices.Remove(pi.playerIndex);
        UIManager.DisablePlayer(pi.playerIndex);
        UIManager.UpdatePlacements();
        pc.gameObject.SetActive(false);
    }

    public void OnDeviceRegain()
    {
        if (SceneManager.GetActiveScene().buildIndex != 2) return;

        if (!GameInfo.playerIndices.Contains(pi.playerIndex)) GameInfo.playerIndices.Add(pi.playerIndex);
        UIManager.EnablePlayer(pi.playerIndex);
        UIManager.UpdatePlacements();
        pc.gameObject.SetActive(true);
    }
}
