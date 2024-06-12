using TMPro;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    internal bool isTimerActive = true;
    private float totalTime = 0;

    [SerializeField]
    private TextMeshProUGUI[] totalTimeTexts;

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
        foreach (TextMeshProUGUI totalTimeText in totalTimeTexts)
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
}
