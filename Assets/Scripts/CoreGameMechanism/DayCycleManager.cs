using UnityEngine;

class DayCycleManager : Singleton<DayCycleManager>
{
    private int FinishedUnitCounter;
    private void Start()
    {
        DaytimeCounter.Instance.OnDayOver.AddListener(EndDay);
    }
    public void EndDay()
    {
        GlobalGameState.Instance.InGameInputAllowed = false;

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
    }
    private void IndicateEndDayRoutineEnd()
    {
        this.FinishedUnitCounter--;
        Debug.LogWarning("Unit finishedCounter decremented");

        if (this.FinishedUnitCounter <= 0)
        {
            //TODO visualise GUI for feeding
            Debug.LogWarning("Units are done preparing for night");
            FeedingManager.Instance.InitiateDayEnd();
        }
    }
    public void RegisterUnit(ActivityStateEndDayRoutine Activity)
    {
        Activity.OnActivityFinished.AddListener(IndicateEndDayRoutineEnd);
    }
}
