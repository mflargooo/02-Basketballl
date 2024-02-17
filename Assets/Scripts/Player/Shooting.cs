using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Shooting : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private Rigidbody rb;
    private int playerID;
    public Slider bar;
    public RectTransform targetIndicator;
    private float acceleration = 0f;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float accelGrowRate = 1f;
    private float targetValue = 0.5f;
    private bool isPressing = false;
    private bool hasPlayed = false;

    [Min(min: 1)]
    [SerializeField] private float shootPower = 1;
    [SerializeField] private GameObject hoop;
    [SerializeField] private Transform scoreZone;
    [SerializeField] private GameObject ballPrefab;

    [SerializeField] private Animator anim;

    private float[] shootRanges;
    bool attemptingShot;

    bool skipThisGame;
    bool insideSmall = false;
    float accelGR;
    float multiplier;

    private bool nextShotDoubled;

    [SerializeField] private GameObject powerupParticles;
    private GameObject powerupPartsInstance;

    // Start is called before the first frame update
    void Start()
    {
        playerID = pc.GetPlayerID();
        ScoreManager sm = hoop.GetComponent<ScoreManager>();
        shootRanges = sm.GetShootRanges();
        ResetGame();
        accelGR = accelGrowRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(!powerupPartsInstance && nextShotDoubled)
        {
            powerupPartsInstance = Instantiate(powerupParticles, transform.position, powerupParticles.transform.rotation);
            powerupPartsInstance.transform.parent = transform;
        }
        else if (powerupPartsInstance && !nextShotDoubled) Destroy(powerupPartsInstance);

        Vector3 horzDisp = new Vector3(hoop.transform.position.x - transform.position.x, 0f, hoop.transform.position.z - transform.position.z);

        if (horzDisp.magnitude <= shootRanges[0])
        {
            GetComponent<PlayerEffects>().SetInvuln(true);
            insideSmall = true;
        }
        else if (insideSmall) GetComponent<PlayerEffects>().SetInvuln(false);

        if (horzDisp.magnitude <= shootRanges[2] && attemptingShot && GameManager.ps[playerID].eggCt > 0 && !hasPlayed && !pc.IsDashing() && pc.GetCanMove())
        {
            multiplier = 1f;
            rb.velocity = Vector3.zero;
            anim.SetFloat("Velocity", 0);
            pc.DisableMovement();

            if (horzDisp.magnitude < shootRanges[0]) multiplier = 3f;
            else if (horzDisp.magnitude < shootRanges[1]) multiplier = 2f;

            hasPlayed = true;
            isPressing = true;
            acceleration = 0f; // reset acc
            SetTargetValue(multiplier);
            bar.gameObject.SetActive(true);
            accelGR = accelGrowRate + (4 - multiplier) * .5f;
        }

        if (isPressing && attemptingShot)
        {
            rb.velocity = Vector3.zero;
            transform.Rotate(Vector3.up * Vector3.SignedAngle(transform.forward, horzDisp, Vector3.up) * Time.deltaTime * 6f);
            if (bar.value >= 1f && acceleration > 0 || bar.value <= 0f && acceleration < 0f)
            {
                acceleration = -acceleration;
                accelGR = -accelGR;
            }
            acceleration = Mathf.Clamp(acceleration + Time.deltaTime * accelGR, -maxAcceleration - (3 + multiplier) * .5f, maxAcceleration + (3 - multiplier) * .5f); 
            bar.value += acceleration * Time.deltaTime; 
        }
        else if (Mathf.Abs(acceleration) > 0)
        {
            isPressing = false;
            bar.value += acceleration * Time.deltaTime;
            acceleration *= 0.8f; 

            if (Mathf.Abs(acceleration) < 0.01f)
            {
                acceleration = 0f;
                CheckSuccess(multiplier);
            }
        }
    }

    void ShootAt()
    {
        Projectile ball = Instantiate(ballPrefab, transform.position + transform.forward, transform.rotation).GetComponent<Projectile>();
        ball.LaunchAt(GameManager.ps[playerID].id, scoreZone.position, shootPower);
        ball.SetNextShotDoubled(nextShotDoubled);
        nextShotDoubled = false;
    }

    void ShootMiss()
    {
        Projectile ball = Instantiate(ballPrefab, transform.position + transform.forward, transform.rotation).GetComponent<Projectile>();
        float angle = Random.Range(0f, 360f);
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * 2f;
        ball.LaunchAt(GameManager.ps[playerID].id, scoreZone.position + offset, shootPower);
        ball.tag = "Untagged";
        SetNextShotDoubled(false);
    }

    void ResetGame()
    {
        bar.value = 0;
        bar.gameObject.SetActive(false);
        hasPlayed = false;
        skipThisGame = false;
    }

    void SetTargetValue(float multiplier)
    {
        targetValue = Random.Range(0.25f, .85f); 
        UpdateTargetIndicatorPosition(multiplier);
    }

    void UpdateTargetIndicatorPosition(float multiplier)
    {
        if (bar != null && targetIndicator != null)
        {
            targetIndicator.sizeDelta = new Vector2(targetIndicator.sizeDelta.x, 15f * multiplier);
            float targetPos = targetValue * bar.GetComponent<RectTransform>().sizeDelta.x;
            targetIndicator.anchoredPosition = new Vector2(targetPos - bar.GetComponent<RectTransform>().sizeDelta.x * 0.5f, targetIndicator.anchoredPosition.y);
        }
    }

    void CheckSuccess(float multiplier)
    {

        float successRangeStart = targetValue - targetIndicator.sizeDelta.y / bar.GetComponent<RectTransform>().sizeDelta.x * .5f - .025f * multiplier;
        float successRangeEnd = targetValue + targetIndicator.sizeDelta.y / bar.GetComponent<RectTransform>().sizeDelta.x * .5f + .025f * multiplier;

        if (!skipThisGame)
        {
            Camera.main.GetComponent<AudioSource>().PlayOneShot(GameManager.charSFX[playerID].hup);
            if (bar.value >= successRangeStart && bar.value <= successRangeEnd)
            {
                ShootAt();
            }
            else
            {
                ShootMiss();
            }
            StartCoroutine(Reset());
        }

        UIManager.UpdateEggs(GameManager.ps[playerID].id, --GameManager.ps[playerID].eggCt);
    }

    IEnumerator Reset()
    {
        bar.gameObject.SetActive(false);
        pc.EnableMovement();
        yield return new WaitForSeconds(1f);
        ResetGame();
    }

    public void SetAttemptingShot(float pressedAmount)
    {
        attemptingShot = pressedAmount != 0;
    }

    public void InterruptGame()
    {
        skipThisGame = true;
        isPressing = false;
        acceleration = 0f;
        ResetGame();
    }
    public void SetAnimator(Animator anim)
    {
        this.anim = anim;
    }

    public void SetNextShotDoubled(bool b)
    {
        nextShotDoubled = b;
    }

    public bool GetNextShotDoubled()
    {
        return nextShotDoubled;
    }
}