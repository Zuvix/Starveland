using UnityEngine;
using UnityEngine.Events;
public class DaytimeCounter : Singleton<DaytimeCounter>
{
    public UnityEvent OnDayOver = new UnityEvent();
    public UnityEvent<int> OnDayStarted = new UnityEvent<int>();
    public UnityEvent<float> OnTimeChanged = new UnityEvent<float>();

    public float DayLength;
    private float DayTimeLeft;
    private int DayCount = 0;
    public bool DayOver { get; private set; }
    void Start()
    {
        OnDayStarted.AddListener(BuildingCrafting.RestoreWork);
        OnDayOver.AddListener(BuildingCrafting.HaltWork);
        StartDay();
    }
    public void StartDay()
    {
        DayCount++;
        DayTimeLeft = DayLength;
        OnDayStarted.Invoke(DayCount);
        DayOver = false;
    }
    private void Update()
    {
        if (!DayOver)
        {
            DayTimeLeft -= Time.deltaTime;
            DayTimeLeft = Mathf.Max(0, DayTimeLeft);
            OnTimeChanged.Invoke(DayTimeLeft);
            if (DayTimeLeft <= 0)
            {
                DayOver = true;
                OnDayOver.Invoke();
            }
        }
    }
}