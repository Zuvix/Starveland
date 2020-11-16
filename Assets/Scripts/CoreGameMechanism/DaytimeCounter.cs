using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DaytimeCounter : Singleton<DaytimeCounter>
{
    public readonly UnityEvent OnDayOver = new UnityEvent();
    public readonly UnityEvent<int> OnDayStarted = new UnityEvent<int>();
    public readonly UnityEvent<float> OnTimeChanged = new UnityEvent<float>();

    public readonly float dayLength=1f;
    private float dayTimeLeft;
    private int dayCount = 0;
    private bool dayOver;

    void Start()
    {
        StartDay();
    }
    public void StartDay()
    {
        dayCount++;
        dayTimeLeft = dayLength;
        OnDayStarted.Invoke(dayCount);
        dayOver = false;
    }
    private void Update()
    {
        if (!dayOver)
        {
            dayTimeLeft -= Time.deltaTime;
            dayTimeLeft = Mathf.Max(0, dayTimeLeft);
            OnTimeChanged.Invoke(dayTimeLeft);
            if (dayTimeLeft <= 0)
            {
                dayOver = true;
                OnDayOver.Invoke();
            }
        }
    }
}
