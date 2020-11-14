using UnityEngine;

class EndDayManager : Singleton<EndDayManager>
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
        /*Unit.UnitPool[0].SetActivity(new ActivityStateEndDayRoutine());
        Unit.UnitPool[1].SetActivity(new ActivityStateEndDayRoutine());
        Unit.UnitPool[2].SetActivity(new ActivityStateEndDayRoutine());*/
    }
    private void StartDay()
    {
        foreach (Unit Unit in Unit.PlayerUnitPool)
        {
            Unit.SetActivity(new ActivityStateIdle());
        }
        DaytimeCounter.Instance.StartDay();
        //TODO hide the GUI
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
        }
    }
    public void RegisterUnit(ActivityStateEndDayRoutine Activity)
    {
        Activity.OnActivityFinished.AddListener(IndicateEndDayRoutineEnd);
    }
}
