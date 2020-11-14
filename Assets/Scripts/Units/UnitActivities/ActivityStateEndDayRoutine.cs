using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
class ActivityStateEndDayRoutine : ActivityState
{
    private UnitCommandMove CommandMove2Storage;
    private UnitCommandDrop CommandDrop2Storage;

    public readonly UnityEvent OnActivityFinished = new UnityEvent();
    public ActivityStateEndDayRoutine()
    {
        EndDayManager.Instance.RegisterUnit(this);
    }
    public override void InitializeCommand(Unit Unit)
    {
        this.CommandMove2Storage = this.CommandToMoveToStorage(Unit);
        Unit.SetCommand(this.CommandMove2Storage);
    }

    public override IEnumerator PerformSpecificAction(Unit Unit)
    {
         if (Unit.CurrentCommand.IsDone(Unit))
         {
             // If unit has walked next to the storage, let's command it to drop resources to it
             if (Unit.CurrentCommand == this.CommandMove2Storage)
             {
                 this.CommandDrop2Storage = new UnitCommandDrop(this.CommandMove2Storage.Target);
                 Unit.SetCommand(this.CommandDrop2Storage);
             }
             // If unit has dropped resources to the storage, let's command it to move to the resource source again
             else if (Unit.CurrentCommand == this.CommandDrop2Storage)
             {
                 //TODO
                 this.FinishedRoutine(Unit);
             }
             else
             {
                 //TODO
                 throw new Exception("Unit's current command is something unexpected");
             }
         }
         else if (!Unit.CurrentCommand.CanBePerformed(Unit))
         {
             // If moving to storage is not possible
             if (Unit.CurrentCommand == this.CommandMove2Storage)
             {
                 yield return Unit.StartCoroutine(Unit.MovementConflictManager.UnableToMoveRoutine(Unit, ActivityStateNull.Instance));
             }
             // If dropping resources at storage is not possible
             else if (Unit.CurrentCommand == this.CommandDrop2Storage)
             {
                 this.FinishedRoutine(Unit);
             }
             else
             {
                 //TODO
                 throw new Exception("Unit's current command is something unexpected");
             }
         }
         else
         {
             yield return Unit.StartCoroutine(Unit.CurrentCommand.PerformAction(Unit));
         }
    }
    private void FinishedRoutine(Unit Unit)
    {
        OnActivityFinished.Invoke();
        Debug.LogWarning("Unit finished End day routine");
        Unit.SetActivity(ActivityStateNull.Instance);
    }
}
