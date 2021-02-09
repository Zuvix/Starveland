using UnityEngine;

class DayCycleManager : Singleton<DayCycleManager>
{
    private int FinishedUnitCounter { get; set; }
    public bool TimeOut { get; private set; }  = false;
    private void Start()
    {
        DaytimeCounter.Instance.OnDayOver.AddListener(EndDay);
        AudioManager.Instance.PlayBackground();
    }

    public void EndDay()
    {
        GlobalGameState.Instance.InGameInputAllowed = false;
        TimeOut = true;
        AudioManager.Instance.StopAll();
        AudioManager.Instance.Play("night");
        UnitManager.Instance.ActionQueue.Clear();
        this.FinishedUnitCounter = Unit.PlayerUnitPool.Count;

        foreach (Unit Unit in Unit.PlayerUnitPool)
        {
            if (Unit.IsInBuilding())
            {
                IndicateEndDayRoutineEnd();
            }
            else
            {
                Unit.OnBuildingEntered.AddListener(IndicateEndDayRoutineEnd);
                Unit.SetActivity(new ActivityStateIdle());
            }
        }

        if (Unit.PlayerUnitPool.Count == 0)
        {
            IndicateEndDayRoutineEnd();
        }

    }
    public void StartDay()
    {
        AudioManager.Instance.PlayBackground();
        foreach (Unit Unit in Unit.PlayerUnitPool)
        {
            Unit.OnBuildingEntered.RemoveListener(IndicateEndDayRoutineEnd);
        }
        DaytimeCounter.Instance.StartDay();
        GlobalGameState.Instance.InGameInputAllowed = true;
        TimeOut = false;
        PlayerPrefs.SetInt(InterSceneVariables.DayCount, PlayerPrefs.GetInt(InterSceneVariables.DayCount) + 1);
    }
    public void IndicateEndDayRoutineEnd()
    {
        this.FinishedUnitCounter--;

        if (this.FinishedUnitCounter <= 0)
        {
            FeedingManager.Instance.InitiateDayEnd();
        }
    }
    public bool GameIsWaitingForPlayerUnits2GoEat()
    {
        return FinishedUnitCounter > 0;
    }
}