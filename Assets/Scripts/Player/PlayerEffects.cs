using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private GameObject[] model;
    [SerializeField] private MonoBehaviour[] disableOnStun;
    private Rigidbody rb;
    private PlayerController pc;
    private Shooting sh;

    [Min(min: .3f)]
    [SerializeField] private float stunTime;
    [SerializeField] private float stunKnockbackVelocity;
    [SerializeField] private float invulnTime;
    [Min(min: .001f)]
    [SerializeField] private float flashTime;

    private bool invuln;

    private void Start()
    {
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        sh = GetComponent<Shooting>();
    }
    public IEnumerator Stun(Vector3 dir)
    {
        if (invuln) yield return null;
        else 
        {
            StartCoroutine(FlashModel(invulnTime + stunTime));
            StartCoroutine(Invuln(invulnTime + stunTime));
            float st = stunTime;
            sh.InterruptGame();
            pc.DisableMovement();
            /*stun anim*/
            foreach (MonoBehaviour script in disableOnStun)
                script.enabled = false;
            while (st > stunTime - .2f)
            {
                rb.velocity = dir * stunKnockbackVelocity;
                st -= Time.deltaTime;
                yield return null;
            }
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(stunTime - .2f);
            yield return null;
            foreach (MonoBehaviour script in disableOnStun)
                script.enabled = true;
            pc.EnableMovement();
            /*stop stun anim*/
        }
    }

    public IEnumerator Invuln(float invulnTime)
    {
        invuln = true;
        yield return new WaitForSeconds(invulnTime);
        invuln = false;
    }

    public IEnumerator FlashModel(float time)
    {
        int cycles = (int)(time / flashTime * .5f);
        while (cycles > 0)
        {
            foreach(GameObject m in model)
                m.SetActive(false);
            yield return new WaitForSeconds(flashTime);
            yield return null;
            foreach (GameObject m in model)
                m.SetActive(true);
            yield return new WaitForSeconds(flashTime);
            cycles--;
        }
        foreach (GameObject m in model)
            m.SetActive(true);
    }
}
