using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Basketball")
        {
            Projectile ball = other.GetComponent<Projectile>();
            int pts = 0;
            float dist = ball.GetStartDistance();
            if (dist < 2f)
                pts = 1;
            else if (dist < 4f)
                pts = 2;
            else if (dist < 6f)
                pts = 3;

            int pid = ball.GetWhoShot();
            GameManager.ps[pid].score += pts;
            UIManager.UpdateScore(pid, GameManager.ps[pid].score);
        }
    }
}
