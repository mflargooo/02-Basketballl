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
    [SerializeField] private TMP_Text shotText;
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

    private float[] shootRanges;
    bool attemptingShot;

    // Start is called before the first frame update
    void Start()
    {
        playerID = GetComponent<PlayerController>().GetPlayerID();
        ScoreManager sm = hoop.GetComponent<ScoreManager>();
        shootRanges = sm.GetShootRanges();
        shotText.gameObject.SetActive(false);
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horzDisp = new Vector3(hoop.transform.position.x - transform.position.x, 0f, hoop.transform.position.z - transform.position.z);
       
        float multiplier = 1;
        if (horzDisp.magnitude <= shootRanges[2] && attemptingShot && GameManager.ps[playerID].eggCt > 0 && !hasPlayed && !pc.IsDashing() && pc.GetCanMove())
        {
            UIManager.UpdateEggs(GameManager.ps[playerID].id, --GameManager.ps[playerID].eggCt);
            pc.DisableMovement();

            if (horzDisp.magnitude < shootRanges[0]) multiplier = 3;
            else if (horzDisp.magnitude < shootRanges[1]) multiplier = 2;

            hasPlayed = true;
            isPressing = true;
            acceleration = 0f; // reset acc
            SetTargetValue(multiplier);
            bar.gameObject.SetActive(true);
        }

        if (isPressing && attemptingShot)
        {
            rb.velocity = Vector3.zero;
            transform.Rotate(Vector3.up * Vector3.SignedAngle(transform.forward, horzDisp, Vector3.up) * Time.deltaTime * 6f);
            if (bar.value >= 1f && acceleration > 0 || bar.value <= 0f && acceleration < 0f)
            {
                acceleration = -acceleration;
                accelGrowRate = -accelGrowRate;
            }
            acceleration = Mathf.Clamp(acceleration + Time.deltaTime * accelGrowRate, -maxAcceleration, maxAcceleration); 
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
                CheckSuccess();
            }
        }
    }

    void ShootAt()
    {
        Projectile ball = Instantiate(ballPrefab, transform.position, transform.rotation).GetComponent<Projectile>();
        ball.LaunchAt(GameManager.ps[playerID].id, scoreZone.position, shootPower);
    }

    void ShootMiss()
    {
        Projectile ball = Instantiate(ballPrefab, transform.position, transform.rotation).GetComponent<Projectile>();
        float angle = Random.Range(0f, 360f);
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        ball.LaunchAt(GameManager.ps[playerID].id, scoreZone.position + offset, shootPower);
        ball.tag = "Untagged";
    }

    void ResetGame()
    {
        bar.value = 0;
        bar.gameObject.SetActive(false);
        hasPlayed = false;
        shotText.gameObject.SetActive(false);
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
            targetIndicator.sizeDelta = new Vector2(targetIndicator.sizeDelta.x, 12.5f * multiplier);
            float targetPos = targetValue * bar.GetComponent<RectTransform>().sizeDelta.x;
            targetIndicator.anchoredPosition = new Vector2(targetPos - bar.GetComponent<RectTransform>().sizeDelta.x * 0.5f, targetIndicator.anchoredPosition.y);
        }
    }

    void CheckSuccess()
    {

        float successRangeStart = targetValue - targetIndicator.sizeDelta.y / bar.GetComponent<RectTransform>().sizeDelta.x * .5f - .05f;
        float successRangeEnd = targetValue + targetIndicator.sizeDelta.y / bar.GetComponent<RectTransform>().sizeDelta.x * .5f + .05f;

        if (bar.value >= successRangeStart && bar.value <= successRangeEnd) 
        {
            ShootAt();
            StartCoroutine(ShowTextAndReset("Success!"));
        }
        else
        {
            ShootMiss();
            StartCoroutine(ShowTextAndReset("Miss!"));
        }
    }

    IEnumerator ShowTextAndReset(string text)
    {
        bar.gameObject.SetActive(false);
        pc.EnableMovement();
        shotText.text = text;
        shotText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        shotText.gameObject.SetActive(false);
        ResetGame();
    }

    public void SetAttemptingShot(float pressedAmount)
    {
        attemptingShot = pressedAmount != 0;
    }

    public void InterruptGame()
    {
        isPressing = false;
        acceleration = 0f;
        ResetGame();
    }
}


