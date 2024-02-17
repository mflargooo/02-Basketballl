using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f; 
    [SerializeField] private float rotateSpeedMultiplier = 10f;
    [SerializeField] private float dashScale = 2f;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldownTime;

    private bool isDashing;

    private float baseMS, dct;

    [SerializeField] private int playerID;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private GameObject dashParticles;

    private Rigidbody rb;

    private Vector3 input;

    private bool canMove = true;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        baseMS = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Input", input.magnitude);
        if (dct > 0f && !isDashing)
        {
            dct -= Time.deltaTime;
        }

        if (!canMove) return;

        if (input.magnitude > 0)
        {
            float angle = Vector3.SignedAngle(transform.forward, input.normalized, transform.up);
            rb.transform.Rotate(Vector3.up * angle * rotateSpeedMultiplier * Time.deltaTime);
        }
        else rb.transform.Rotate(Vector3.zero);

        if(!isDashing) rb.velocity = input.normalized * moveSpeed;
        anim.SetFloat("Velocity", rb.velocity.magnitude);
    }

    public int GetPlayerID()
    {
        return playerID;
    }

    public void SetInputVector(Vector2 input)
    {
        this.input = new Vector3(input.x, 0f, input.y);
    }

    public void SetDashPressed(float input)
    {
        if (input != 0 && dct <= 0f && canMove)
        {
            Camera.main.GetComponent<AudioSource>().PlayOneShot(dashClip);
            GameObject temp = Instantiate(dashParticles, transform.position + Vector3.up * .375f - transform.forward * .25f, transform.rotation);
            Destroy(temp, .5f);
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        dct = dashCooldownTime;
        rb.velocity = (input.magnitude > 0 ? input : transform.forward) * baseMS * dashScale;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }

    public void DisableMovement()
    {
        canMove = false;
        anim.SetBool("CanMove", false);
    }

    public void EnableMovement()
    {
        canMove = true;
        anim.SetBool("CanMove", true);
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public void SetAnimator(Animator anim)
    {
        this.anim = anim;
    }
}

