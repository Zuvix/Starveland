using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameplayEvents : Singleton<GameplayEvents>
{
    public TMP_Text timerTxt;
    public TMP_Text dayTxt;
    public UnityEvent OnDayOver = new UnityEvent();
    public UnityEvent OnDayStarted = new UnityEvent();
    public float daylength=180f;
    public int dayCount=0;
    
    // Start is called before the first frame update
    void Start()
    {
        StartNewDay();
    }
    public void StartNewDay()
    {
        dayCount++;
        StartCoroutine("DayCounter");
        dayTxt.text = "Day " + dayCount;
        OnDayStarted.Invoke();
    }
    // Update is called once per frame
    IEnumerator DayCounter()
    {
        float timeLeft = daylength;
        while (timeLeft >= 0)
        {
            timeLeft -= 1;
            DisplayTime(timeLeft);
            yield return new WaitForSeconds(1f);
        }
        OnDayOver.Invoke();
    }
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerTxt.text = string.Format("Time to night: {0:00}:{1:00}", minutes, seconds);
    }
}
