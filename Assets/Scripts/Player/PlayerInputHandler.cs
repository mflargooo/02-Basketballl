using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private GameManager gm;
    private PlayerInput pi;
    private PlayerController pc;
    private Shooting sh;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
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
}
