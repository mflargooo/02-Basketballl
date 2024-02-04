using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private float smallRingRadius;
    [SerializeField] private float medRingRadius;
    [SerializeField] private float largeRingRadius;

    [SerializeField] private GameObject[] rings;

    private void Start()
    {
        rings[0].transform.localScale = new Vector3(smallRingRadius, smallRingRadius, 1f);
        rings[1].transform.localScale = new Vector3(medRingRadius, medRingRadius, 1f);
        rings[2].transform.localScale = new Vector3(largeRingRadius, largeRingRadius, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Basketball")
        {
            Projectile ball = other.GetComponent<Projectile>();
            int pts = 0;
            float dist = ball.GetStartDistance();
            print(dist);
            if (dist < smallRingRadius * .5f - .125f)
                pts = 1;
            else if (dist < medRingRadius * .5f - .125f)
                pts = 2;
            else if (dist < largeRingRadius * .5f + .25f)
                pts = 3;

            int pid = ball.GetWhoShot();
            GameManager.ps[pid].score += pts;
            UIManager.UpdateScore(pid, GameManager.ps[pid].score);
        }
    }
}
