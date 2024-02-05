using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Shooting : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int playerID;
    public Slider bar;
    public RectTransform targetIndicator;
    [SerializeField] private TMP_Text shotText;
    private float acceleration = 0f;
    [SerializeField] private float accelGrowRate = 1f;
    private float targetValue = 0.5f;
    private bool isPressing = false;
    private bool hasPlayed = false;

    [Min(min: 1)]
    [SerializeField] private float shootPower = 1;
    [SerializeField] private GameObject hoop;
    [SerializeField] private Transform scoreZone;
    [SerializeField] private GameObject ballPrefab;

    [SerializeField] private UIManager uiManager;
    
    private float maxShootDist;

    // Start is called before the first frame update
    void Start()
    {
        maxShootDist = hoop.GetComponent<ScoreManager>().GetMaxShootDist();
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horzDisp = new Vector3(hoop.transform.position.x - transform.position.x, 0f, hoop.transform.position.z - transform.position.z);
        if (horzDisp.magnitude <= maxShootDist && Input.GetKeyDown(KeyCode.Space) && GameManager.ps[playerID].eggCt > 0 && !hasPlayed)
        {
            hasPlayed = true;
            isPressing = true;
            acceleration = 0f; // reset acc
            bar.gameObject.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            isPressing = false;
        }

        if (isPressing)
        {
            acceleration += Time.deltaTime * accelGrowRate; 
            bar.value += acceleration * Time.deltaTime; 
        }
        else if (acceleration > 0)
        {
            bar.value += acceleration * Time.deltaTime;
            acceleration *= 0.8f; 

            if (acceleration < 0.01f)
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
        SetTargetValue();
        hasPlayed = false;
        shotText.gameObject.SetActive(false);
    }

    void SetTargetValue()
    {
        targetValue = Random.Range(0.25f, 1f); 
        UpdateTargetIndicatorPosition();
    }

    void UpdateTargetIndicatorPosition()
    {
        if (bar != null && targetIndicator != null)
        {
            float targetPos = targetValue * bar.GetComponent<RectTransform>().sizeDelta.x;
            targetIndicator.anchoredPosition = new Vector2(targetPos - bar.GetComponent<RectTransform>().sizeDelta.x * 0.5f, targetIndicator.anchoredPosition.y);
        }
    }

    void CheckSuccess()
    {
        UIManager.UpdateEggs(GameManager.ps[playerID].id, --GameManager.ps[playerID].eggCt);
        if (Mathf.Abs(bar.value - targetValue) < 0.05f) 
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
        shotText.text = text;
        shotText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        ResetGame();
    }
}


