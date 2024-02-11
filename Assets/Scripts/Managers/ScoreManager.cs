using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private float smallRingRadius;
    [SerializeField] private float medRingRadius;
    [SerializeField] private float largeRingRadius;

    [SerializeField] private GameObject[] rings;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swooshClip;

    [SerializeField] private ParticleSystem ps;
    ParticleSystem.MainModule main;

    private float[] shootingRanges;
    private void Awake()
    {
        rings[0].transform.localScale = new Vector3(smallRingRadius * 2f, smallRingRadius * 2f, 1f);
        rings[1].transform.localScale = new Vector3(medRingRadius * 2f, medRingRadius * 2f, 1f);
        rings[2].transform.localScale = new Vector3(largeRingRadius * 2f, largeRingRadius * 2f, 1f);

        shootingRanges = new float[3] { smallRingRadius - .125f, medRingRadius - .125f, largeRingRadius};
        main = ps.main;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Basketball")
        {
            Projectile ball = other.GetComponent<Projectile>();
            int pts = 0;
            float dist = ball.GetStartDistance();

            if (dist < smallRingRadius - .125f)
                pts = 1;
            else if (dist < medRingRadius - .125f)
                pts = 2;
            else if (dist < largeRingRadius + .25f)
                pts = 3;

            int pid = ball.GetWhoShot();
            GameManager.ps[pid].score += pts;
            UIManager.UpdateScore(pid, GameManager.ps[pid].score);
            main.startColor = GameManager.pc[pid];
            ps.Play();
            Destroy(ball.gameObject, .1f);
            audioSource.PlayOneShot(swooshClip);
        }
    }

    public float[] GetShootRanges()
    {
        return shootingRanges;
    }
}
