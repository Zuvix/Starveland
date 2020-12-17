using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Building : CellObject
{
    public List<Resource> ConstructionCost;
    public List<Unit> CurrentVisitors = new List<Unit>();
    public UnityEvent<Building> OnVisitorsChanged = new UnityEvent<Building>();
    public void Enter(Unit Unit)
    {
        this.CurrentVisitors.Add(Unit);
        Unit.EnterBuilding(this);
        Unit.ToggleVisibility(false);
        OnVisitorsChanged.Invoke(this);
    }
    public void Leave(Unit Unit)
    {
        this.CurrentVisitors.Remove(Unit);
        MapCell ExitCell = CurrentCell.GetRandomUnitEnterableNeighbour();
        if (ExitCell != null)
        {
            Unit.LeaveBuilding(ExitCell);
            ExitCell.Map.CenterObject(ExitCell.x, ExitCell.y, Unit.gameObject);
            Unit.ToggleVisibility(true);
        }
        else
        {
            Debug.LogError("Unit tried to leave building, but there was no space.");
            this.CurrentVisitors.Add(Unit);
        }
        OnVisitorsChanged.Invoke(this);
    }
    public void LeaveDead(Unit Unit)
    {
        this.CurrentVisitors.Remove(Unit);
        Unit.LeaveBuildingDead();
        OnVisitorsChanged.Invoke(this);
    }
}