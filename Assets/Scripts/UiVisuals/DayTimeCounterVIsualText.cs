using UnityEngine;
using UnityEngine.Events;
using TMPro;

class DayTimeCounterVIsualText : Singleton<DayTimeCounterVIsualText>
{
    public TMP_Text timerTxt;
    public TMP_Text dayCountTxt;

    void Start()
    {
        DaytimeCounter.Instance.OnTimeChanged.AddListener(UpdateTimerText);
        DaytimeCounter.Instance.OnDayStarted.AddListener(UpdateDayCountText);
    }
    private void UpdateTimerText(float TimeToDisplay)
    {
        float minutes = Mathf.FloorToInt(TimeToDisplay / 60);
        float seconds = Mathf.FloorToInt(TimeToDisplay % 60);

        timerTxt.text = $"{minutes:00}:{seconds:00}";
    }
    private void UpdateDayCountText(int Day)
    {
        dayCountTxt.text = $"{Day}";
    }
}
