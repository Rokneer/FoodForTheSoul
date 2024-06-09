using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    internal bool isTimerActive = true;
    private float totalTime = 0;

    [SerializeField]
    private TextMeshProUGUI totalTimeText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (isTimerActive)
        {
            if (totalTime >= 0)
            {
                totalTime += Time.deltaTime;
            }
            else
            {
                totalTime = 0;
            }
            DisplayTime();
        }
    }

    internal void DisplayTime()
    {
        if (totalTime < 0)
        {
            totalTime = 0;
        }
        float minutes = Mathf.FloorToInt(totalTime / 60);
        float seconds = Mathf.FloorToInt(totalTime % 60);
        totalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
