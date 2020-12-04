using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DaytimeCounter : Singleton<DaytimeCounter>
{
    public UnityEvent OnDayOver;
    public UnityEvent<int> OnDayStarted;
    public UnityEvent<float> OnTimeChanged;

    public float dayLength;
    private float dayTimeLeft;
    private int dayCount = 0;
    private bool dayOver;

    private void Awake()
    {
        OnDayOver = new UnityEvent();
        OnDayStarted = new UnityEvent<int>();
        OnTimeChanged = new UnityEvent<float>();
    }
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
