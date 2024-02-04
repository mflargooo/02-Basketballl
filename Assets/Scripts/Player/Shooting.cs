using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class Shooting : MonoBehaviour
{
    public Slider bar;
    public RectTransform targetIndicator;
    public GameObject successUI;
    private float acceleration = 0f;
    [SerializeField] private float accelGrowRate = 1f;
    private float targetValue = 0.5f;
    private bool isPressing = false;
    private bool hasPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasPlayed)
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

    void ResetGame()
    {
        bar.value = 0;
        bar.gameObject.SetActive(false);
        SetTargetValue();
        hasPlayed = false;
        successUI.SetActive(false);
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

        if (Mathf.Abs(bar.value - targetValue) < 0.05f) 
        {
            StartCoroutine(ShowSuccessAndReset());
        }
        else
        {
            ResetGame();
        }
    }

    IEnumerator ShowSuccessAndReset()
    {
        successUI.SetActive(true);
        yield return new WaitForSeconds(1);
        ResetGame();
    }
}


