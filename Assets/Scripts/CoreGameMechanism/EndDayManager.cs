using UnityEngine;

class EndDayManager : Singleton<EndDayManager>
{
    private int FinishedUnitCounter;
    private void Start()
    {
        DaytimeCounter.Instance.OnDayOver.AddListener(EndDay);
    }
    private void EndDay()
    {
        GlobalGameState.Instance.InGameInputAllowed = false;

        UnitManager.Instance.ActionQueue.Clear();
        UnitManager.Instance.IdleUnits.Clear();
        this.FinishedUnitCounter = Unit.UnitPool.Count;
        /*foreach (Unit Unit in Unit.UnitPool)
        {
            Unit.SetActivity(new ActivityStateIdle());
        }*/
        Unit.UnitPool[0].SetActivity(new ActivityStateEndDayRoutine());
        /*Unit.UnitPool[1].SetActivity(new ActivityStateEndDayRoutine());*/
        /*Unit.UnitPool[2].SetActivity(new ActivityStateEndDayRoutine());*/
        Debug.LogWarning("Units are all assigned to end their daily tasks");
    }
    private void StartDay()
    {
        foreach (Unit Unit in Unit.UnitPool)
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
