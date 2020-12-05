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
        UnitManager.Instance.IdleUnits.Clear();
        this.FinishedUnitCounter = Unit.PlayerUnitPool.Count;

        foreach (Unit Unit in Unit.PlayerUnitPool)
        {
            Unit.SetActivity(new ActivityStateEndDayRoutine());
        }

        if (this.FinishedUnitCounter == 0)
        {
            IndicateEndDayRoutineEnd();
        }
    }
    public void StartDay()
    {
        foreach (Unit Unit in Unit.PlayerUnitPool)
        {
            Unit.SetActivity(new ActivityStateIdle());
        }
        DaytimeCounter.Instance.StartDay();
        GlobalGameState.Instance.InGameInputAllowed = true;
        TimeOut = false;
    }
    public void IndicateEndDayRoutineEnd()
    {
        this.FinishedUnitCounter--;
        Debug.Log($"Unit finishedCounter decremented to {this.FinishedUnitCounter}");

        if (this.FinishedUnitCounter <= 0)
        {
            //TODO visualise GUI for feeding
            Debug.Log("Units are done preparing for night");
            FeedingManager.Instance.InitiateDayEnd();
        }
    }
    public void RegisterUnit(ActivityStateEndDayRoutine Activity)
    {
        Activity.OnActivityFinished.AddListener(IndicateEndDayRoutineEnd);
    }
    public bool GameIsWaitingForPlayerUnits2GoEat()
    {
        return FinishedUnitCounter > 0;
    }
}