using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class ActivityState
{
    public IEnumerator PerformAction(Unit Unit)
    {
        if (Unit.ChangeActivity())
        {
            yield return null;
        }
        else if (Unit.IsInBuilding())
        {
            HandleInBuildingAction(Unit);
        }
        else
        {
            yield return PerformSpecificAction(Unit);
        }
    }
    public abstract IEnumerator PerformSpecificAction(Unit Unit);
    public virtual void HandleInBuildingAction(Unit Unit)
    {
        Building Building = Unit.CurrentBuilding;
        Unit.CurrentBuilding.Leave(Unit);
        Building.CreatePopup(Unit.GetComponent<SpriteRenderer>().sprite, -1);
    }
    public abstract void InitializeCommand(Unit Unit);
    public virtual ActivityState SetCommands(Unit Unit, Skill Skill)
    {
        return null;
    }

    public UnitCommandMove CommandToMoveToStorage(Unit Unit)
    {
        (List<MapCell>, MapCell) Temp = PathFinding.Instance.FindPath(Unit.CurrentCell, MapControl.Instance.StorageList, PathFinding.EXCLUDE_LAST);
        List<MapCell> Path = Temp.Item1;
        MapCell ClosestStorage = Temp.Item2;

        UnitCommandMove Result = null;
        // Oh no, it's not possible to get to any Storage?
        if (Path == null)
        {
            Debug.Log("Neviem najst cestu nastavujem sa na idle!");
            Unit.SetActivity(new ActivityStateIdle());
        }
        // We found a path to Storage
        else
        {
            Result = new UnitCommandMove(ClosestStorage, Path);
            Unit.SetCommand(Result);
        }
        return Result;
    }
    public abstract bool IsCancellable();
    public abstract bool IsInterruptibleByAttack();
}