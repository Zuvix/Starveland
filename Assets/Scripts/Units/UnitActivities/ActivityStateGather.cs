using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

class ActivityStateGather : ActivityState
{
    private UnitCommandMove CommandMove2Resource;
    private UnitCommandGather CommandGatherFromResource;
    private UnitCommandMove CommandMove2Storage;
    private UnitCommandDrop CommandDrop2Storage;
    private MapCell Target;
    private ResourceSourceGeneric originalResourceSource;

    public ActivityStateGather(MapCell Target) : base()
    {
        this.Target = Target;
        this.originalResourceSource = (ResourceSourceGeneric)this.Target.CurrentObject;
    }

    public override ActivityState SetCommands(Unit Unit, Skill Skill)
    {
        Unit.SetSprite(Skill.unitSprite);

        List<MapCell> Path2Resource = PathFinding.Instance.FindPath(Unit.CurrentCell, this.Target, PathFinding.EXCLUDE_LAST);

        this.CommandMove2Resource = new UnitCommandMove(this.Target, Path2Resource);

        this.CommandGatherFromResource = new UnitCommandGather(this.Target, Skill, this.originalResourceSource);
        this.CommandMove2Storage = null;
        this.CommandDrop2Storage = null;
        return this;
    }

    public override IEnumerator PerformSpecificAction(Unit Unit)
    {
        if (Unit.CurrentCommand.IsDone(Unit))
        {
            // If Unit arrived next to resource, let's command it to gather
            if (Unit.CurrentCommand == this.CommandMove2Resource)
            {
                if (this.Target.CurrentObject == (CellObject)this.originalResourceSource)
                {
                    Unit.SetCommand(this.CommandGatherFromResource);
                }
                else
                {
                    Unit.SetActivity(new ActivityStateIdle());
                }
            }
            // If Unit is finished gathering (full inventory), let's command it to move to storage
            else if (Unit.CurrentCommand == this.CommandGatherFromResource)
            {
                this.CommandMove2Storage = this.CommandToMoveToStorage(Unit);
                Unit.MovementConflictManager.RefreshRemainingRetryCounts();
            }
            // If unit has walked next to the storage, let's command it to drop resources to it
            else if (Unit.CurrentCommand == this.CommandMove2Storage)
            {
                this.CommandDrop2Storage = new UnitCommandDrop(this.CommandMove2Storage.Target);
                Unit.SetCommand(this.CommandDrop2Storage);
            }
            // If unit has dropped resources to the storage, let's command it to move to the resource source again
            else if (Unit.CurrentCommand == this.CommandDrop2Storage)
            {
                // If target resource is depleted, there is no reason to move to it
                CellObject TargetResourceSource = (CellObject)this.Target.CurrentObject;
                if (TargetResourceSource == null || (TargetResourceSource is ResourceSourceGeneric && ((ResourceSourceGeneric)TargetResourceSource).IsDepleted()))
                {
                    Unit.SetActivity(new ActivityStateIdle());
                }
                // The target resource is not depleted, let's move to it
                else
                {
                    this.CommandMove2Resource = new UnitCommandMove(this.CommandMove2Resource.Target, PathFinding.Instance.FindPath(Unit.CurrentCell, this.CommandMove2Resource.Target, PathFinding.EXCLUDE_LAST));
                    Unit.SetCommand(this.CommandMove2Resource);
                    Unit.MovementConflictManager.RefreshRemainingRetryCounts();
                }
            }
            else
            {
                Debug.LogError($"Unit's current command is done, but is something unexpected: {Unit.CurrentCommand}. Its current activity is: {Unit.CurrentActivity}");
                Unit.SetDefaultActivity();
            }
        }
        else if (!Unit.CurrentCommand.CanBePerformed(Unit))
        {
            // If moving to resource is not possible
            if (Unit.CurrentCommand == this.CommandMove2Resource)
            {
                yield return Unit.StartCoroutine(Unit.MovementConflictManager.UnableToMoveRoutine(Unit));
            }
            // If gathering from resource is not possible
            else if (Unit.CurrentCommand == this.CommandGatherFromResource)
            {
                if (Unit.CarriedResource.IsDepleted())
                {
                    Unit.SetActivity(new ActivityStateIdle());
                }
                else
                {
                    this.CommandMove2Storage = this.CommandToMoveToStorage(Unit);
                }
            }
            // If moving to storage is not possible
            else if (Unit.CurrentCommand == this.CommandMove2Storage)
            {
                yield return Unit.StartCoroutine(Unit.MovementConflictManager.UnableToMoveRoutine(Unit));
            }
            // If dropping resources at storage is not possible
            else if (Unit.CurrentCommand == this.CommandDrop2Storage)
            {
                if (MapControl.Instance.StorageList.Count > 0)
                {
                    UnitCommandMove NewMoveCommand = this.CommandToMoveToStorage(Unit);
                    if (NewMoveCommand != null)
                    {
                        this.CommandMove2Storage = NewMoveCommand;
                    }
                    else
                    {
                        Unit.SetActivity(new ActivityStateIdle());
                    }
                }
                else
                {
                    Unit.SetActivity(new ActivityStateIdle());
                }
            }
            else
            {
                //TODO
                Debug.LogError($"Unit's current command cannot be performed and something unexpected: {Unit.CurrentCommand}. Its current activity is: {Unit.CurrentActivity}");
                Unit.SetDefaultActivity();
            }
        }
        else
        {
            yield return Unit.StartCoroutine(Unit.CurrentCommand.PerformAction(Unit));
        }
    }
    public override void InitializeCommand(Unit Unit)
    {
        ResourceSourceGeneric TargetResourceSource = (ResourceSourceGeneric)this.Target.CurrentObject;
        if (
            Unit.CarriedResource.IsDepleted()
                ||
            (!Unit.InventoryFull() && TargetResourceSource != null  && (!(TargetResourceSource is ResourceSource) || (TargetResourceSource is ResourceSource && Unit.CarriedResource.itemInfo == ((ResourceSource)TargetResourceSource).resource.itemInfo)))
           )
        {
            Unit.SetCommand(this.CommandMove2Resource);
        }
        else
        {
            this.CommandMove2Storage = this.CommandToMoveToStorage(Unit);
        }
    }

    public override bool IsCancellable()
    {
        return true;
    }

    public override bool IsInterruptibleByAttack()
    {
        return true;
    }
}
