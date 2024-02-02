using UnityEngine;
using UnityEngine.UI;

public class ShootingWithReset : MonoBehaviour
{
    public Slider bar; 
    public RectTransform targetIndicator; 
    public GameObject successUI; 
    public float moveSpeed = 0.5f; 
    private float targetValue = 0.5f; 
    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        ResetGame(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moving = !moving; 
        }
        if (moving)
        {
            bar.value = Mathf.Lerp(bar.value, targetValue, Time.deltaTime * moveSpeed);

            if (Mathf.Abs(bar.value - targetValue) < 0.05f)//0.05
            {
                ShowSuccessUI(); 
                ResetGame(); 
            }
        }
    }
    void ResetGame()
    {
        moving = false; 
        bar.value = 0; 
        SetTargetValue(); 
        successUI.SetActive(false); 
    }
    void SetTargetValue()
    {
        targetValue = Random.Range(0f, 1f); 
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

    void ShowSuccessUI()
    {
        successUI.SetActive(true); 
    }
}

