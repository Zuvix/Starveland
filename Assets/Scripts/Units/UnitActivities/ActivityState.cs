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
        else
        {
            yield return PerformSpecificAction(Unit);
        }
    }
    public abstract IEnumerator PerformSpecificAction(Unit Unit);
    public virtual void InitializeCommand(Unit Unit)
    {
        Unit.SetCommand(null);
    }
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
}
