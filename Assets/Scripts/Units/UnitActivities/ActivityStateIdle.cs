using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class ActivityStateIdle : ActivityState
{
    private UnitCommandMove MoveToHouseCommand;
    private UnitCommandIdle IdleCommand;
    private UnitCommandDrop DropCommand = null;
    private UnitCommandEnterBuilding EnterBuildingCommand;

    public ActivityStateIdle() : base()
    {
        //this.MoveToHouseCommand = new UnitCommandMove()
        this.IdleCommand = new UnitCommandIdle();
    }
    public override void InitializeCommand(Unit Unit)
    {
        this.MoveToHouseCommand = this.CommandToMoveToStorage(Unit);
        Unit.SetSprite();
    }
    public override IEnumerator PerformSpecificAction(Unit Unit)
    {
        //TODO perhaps if i have something in inventory, I should go drop it?
        if (Unit.CurrentCommand.IsDone(Unit))
        {
            // If Unit arrived next house, command it to stay idle
            if (Unit.CurrentCommand == this.MoveToHouseCommand)
            {
                if (Unit.CarriedResource.IsDepleted())
                {
                    EnterBuildingCommand = new UnitCommandEnterBuilding(MoveToHouseCommand.Target);
                    Unit.SetCommand(EnterBuildingCommand);
                }
                else
                {
                    this.DropCommand = new UnitCommandDrop(MoveToHouseCommand.Target);
                    Unit.SetCommand(this.DropCommand);
                }
            }
            else if (Unit.CurrentCommand == this.DropCommand)
            {
                //Unit.SetCommand(this.IdleCommand);
                EnterBuildingCommand = new UnitCommandEnterBuilding(DropCommand.Target);
                Unit.SetCommand(EnterBuildingCommand);
            }
            else if (Unit.CurrentCommand == this.EnterBuildingCommand)
            {
                Unit.SetCommand(this.IdleCommand);
            }
            // If Unit is finished idling for whatever reason, try to move it to nearest house again
            else if (Unit.CurrentCommand == this.IdleCommand)
            {
                this.MoveToHouseCommand = this.CommandToMoveToStorage(Unit);
            }
            else
            {
                //TODO
                throw new Exception("Unit's current command is something unexpected");
            }
            // If Unit is done doing something, we set new command to queue.
            // However, we were expected to do something because PerformAction was called, so we need to retry
            //this.PerformSpecificAction(Unit);
        }
        else if (!Unit.CurrentCommand.CanBePerformed(Unit))
        {
            // If moving to house
            if (Unit.CurrentCommand == this.MoveToHouseCommand)
            {
                yield return Unit.StartCoroutine(Unit.MovementConflictManager.UnableToMoveRoutine(Unit));
            }
            else if (Unit.CurrentCommand == this.DropCommand || Unit.CurrentCommand == this.EnterBuildingCommand)
            {
                UnitCommandMove NewMoveCommand = this.CommandToMoveToStorage(Unit);
                if (NewMoveCommand != null)
                {
                    this.MoveToHouseCommand = NewMoveCommand;
                    Unit.SetCommand(this.MoveToHouseCommand);
                }
                else
                {
                    Unit.SetCommand(new UnitCommandIdle());
                }
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
    public override bool IsCancellable()
    {
        return false;
    }
    public override bool IsInterruptibleByAttack()
    {
        return false;
    }
    public override void HandleInBuildingAction(Unit Unit) {}
}
