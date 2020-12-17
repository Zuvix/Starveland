using UnityEngine;

class DayCycleManager : Singleton<DayCycleManager>
{
    private int FinishedUnitCounter { get; set; }
    public bool TimeOut { get; private set; }  = false;
    private void Start()
    {
        DaytimeCounter.Instance.OnDayOver.AddListener(EndDay);
    }

    public void EndDay()
    {
        GlobalGameState.Instance.InGameInputAllowed = false;
        TimeOut = true;

        UnitManager.Instance.ActionQueue.Clear();
        //UnitManager.Instance.IdleUnits.Clear();
        this.FinishedUnitCounter = UnitManager.Instance.UnitPool.Count;

        foreach (Unit Unit in UnitManager.Instance.PlayerUnitPool)
        {
            //Unit.SetActivity(new ActivityStateEndDayRoutine());
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

        if (UnitManager.Instance.PlayerUnitPool.Count == 0)
        {
            IndicateEndDayRoutineEnd();
        }

    }
    public void StartDay()
    {
        foreach (Unit Unit in UnitManager.Instance.PlayerUnitPool)
        {
            Unit.OnBuildingEntered.RemoveListener(IndicateEndDayRoutineEnd);
        }
        DaytimeCounter.Instance.StartDay();
        GlobalGameState.Instance.InGameInputAllowed = true;
        TimeOut = false;
        PlayerPrefs.SetInt("DaysPassed", PlayerPrefs.GetInt("DaysPassed") + 1);
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