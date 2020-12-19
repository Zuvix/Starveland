using System.Collections;
using UnityEngine;

public class ActivityStateMoveToSpawnPosition : ActivityState
{
    private MapCell TargetCell;
    private UnitCommandMove CommandMove2Position;

    public ActivityStateMoveToSpawnPosition(MapCell TargetCell)
    {
        this.TargetCell = TargetCell;
    }
    public override IEnumerator PerformSpecificAction(Unit Unit)
    {
        //TODO perhaps if i have something in inventory, I should go drop it?
        if (Unit.CurrentCommand.IsDone(Unit))
        {
            // If Unit arrived next house, command it to stay idle
            if (Unit.CurrentCommand == this.CommandMove2Position)
            {
                Unit.SetDefaultActivity();
            }
            else
            {
                Debug.LogError($"Unit's current command is done, but is something unexpected: {Unit.CurrentCommand}. Its current activity is: {Unit.CurrentActivity}");
            }
        }
        else if (!Unit.CurrentCommand.CanBePerformed(Unit))
        {
            // If moving to house
            if (Unit.CurrentCommand == this.CommandMove2Position)
            {
                yield return Unit.StartCoroutine(Unit.MovementConflictManager.UnableToMoveRoutine(Unit));
            }
            else
            {
                Debug.LogError($"Unit's current command is done, but is something unexpected: {Unit.CurrentCommand}. Its current activity is: {Unit.CurrentActivity}");
            }
        }
        else
        {
            yield return Unit.StartCoroutine(Unit.CurrentCommand.PerformAction(Unit));
        }
    }
    public override void InitializeCommand(Unit Unit)
    {
        this.CommandMove2Position = new UnitCommandMove(TargetCell, PathFinding.Instance.FindPath(Unit.CurrentCell, TargetCell));
        Unit.SetCommand(this.CommandMove2Position);
    }
    public override bool IsCancellable()
    {
        return false;
    }
    public override bool IsInterruptibleByAttack()
    {
        return true;
    }
}