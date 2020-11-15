using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class ActivityStateIdle : ActivityState
{
    private UnitCommandMove MoveToHouseCommand;
    private UnitCommandIdle IdleCommand;

    public ActivityStateIdle() : base()
    {
        //this.MoveToHouseCommand = new UnitCommandMove()
        this.IdleCommand = new UnitCommandIdle();
    }
    public override void InitializeCommand(Unit Unit)
    {
        base.InitializeCommand(Unit);
        this.CommandToMoveToIdleHouse(Unit);
    }
    public override IEnumerator PerformAction(Unit Unit)
    {
        //TODO perhaps if i have something in inventory, I should go drop it?
        if (Unit.CurrentCommand.IsDone(Unit))
        {
            // If Unit arrived next house, command it to stay idle
            if (Unit.CurrentCommand == this.MoveToHouseCommand)
            {
                Unit.SetCommand(this.IdleCommand);
            }
            // If Unit is finished idling for whatever reason, try to move it to nearest house again
            else if (Unit.CurrentCommand == this.IdleCommand)
            {
                Debug.LogWarning("bol som idle a znova hladam cestu do skladu");
                this.CommandToMoveToIdleHouse(Unit);
            }
            else
            {
                //TODO
                throw new Exception("Unit's current command is something unexpected");
            }
            // If Unit is done doing something, we set new command to queue.
            // However, we were expected to do something because PerformAction was called, so we need to retry
            this.PerformAction(Unit);
        }
        else if (!Unit.CurrentCommand.CanBePerformed(Unit))
        {
            // If moving to house
            if (Unit.CurrentCommand == this.MoveToHouseCommand)
            {
                yield return Unit.StartCoroutine(Unit.MovementConflictManager.UnableToMoveRoutine(Unit));
            }
            // If gathering from resource is not possible
            else if (Unit.CurrentCommand == this.IdleCommand)
            {
                // TODO
                throw new Exception("Unit's current command is something unexpected");
            }
            // If moving to storage is not possible
            else
            {
                //TODO
                throw new Exception("Unit's current command is something unexpected when it was idle");
            }
        }
        else
        {
            yield return Unit.StartCoroutine(Unit.CurrentCommand.PerformAction(Unit));
        }
    }
    private void CommandToMoveToIdleHouse(Unit Unit)
    {
        (List<MapCell>, MapCell) Temp = PathFinding.Instance.FindPath(Unit.CurrentCell, MapControl.Instance.StorageList, PathFinding.EXCLUDE_LAST);
        List<MapCell> Path = Temp.Item1;
        MapCell ClosestStorage = Temp.Item2;

        // Oh no, it's not possible to get to any Storage?
        if (Path == null)
        {
            Debug.LogWarning("Initial path to storage not found");
            Unit.SetCommand(this.IdleCommand);
        }
        // We found a path to Storage
        else
        {
            this.MoveToHouseCommand = new UnitCommandMove(ClosestStorage, Path);
            Unit.SetCommand(this.MoveToHouseCommand);
        }
    }
}
