using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private GameObject[] model;
    private Rigidbody rb;
    private PlayerController pc;
    private Shooting sh;

    [Min(min: .3f)]
    [SerializeField] private float stunTime;
    [SerializeField] private float stunKnockbackVelocity;
    [SerializeField] private float knockbackTime = .2f;
    [SerializeField] private float invulnTime;
    [Min(min: .001f)]
    [SerializeField] private float flashTime;

    [SerializeField] private Animator anim;
    [SerializeField] private AnimationClip stun;

    public bool invuln { get; private set; }

    private void Start()
    {
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        sh = GetComponent<Shooting>();
    }
    public IEnumerator Stun(Transform hitBy)
    {
        if (invuln) yield return null;
        else 
        {
            StartCoroutine(FlashModel(invulnTime + stunTime));
            StartCoroutine(Invuln(invulnTime + stunTime));
            Camera.main.GetComponent<AudioSource>().PlayOneShot(GameManager.charSFX[pc.GetPlayerID()].hit);
            anim.Play(stun.name);
            Vector3 dir = hitBy.forward;
            transform.rotation = Quaternion.LookRotation((hitBy.position - transform.position).normalized, transform.up);
            sh.InterruptGame();
            pc.DisableMovement();
            float kbTime = 0f;
            /*stun anim*/
            while (kbTime < knockbackTime)
            {
                rb.velocity = dir * stunKnockbackVelocity;
                kbTime += Time.deltaTime;
                yield return null;
            }
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(stunTime - .2f);
            yield return null;
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

    public void SetInvuln(bool b)
    {
        invuln = b;
    }
    public void SetAnimator(Animator anim)
    {
        this.anim = anim;
    }
}
