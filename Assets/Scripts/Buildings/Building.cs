using System.Collections.Generic;

public abstract class Building : CellObject
{
    public List<Resource> ConstructionCost;
    public List<Unit> CurrentVisitors = new List<Unit>();

    public void Enter(Unit Unit)
    {
        this.CurrentVisitors.Add(Unit);
        Unit.EnterBuilding(this);
        Unit.ToggleVisibility(false);
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
    }
    public void LeaveDead(Unit Unit)
    {
        this.CurrentVisitors.Remove(Unit);
        Unit.LeaveBuildingDead();
    }
}