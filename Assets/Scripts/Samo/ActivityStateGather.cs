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

    private int RemainingMovementTries;
    private int RemainingNewPathFindTries;

    private static readonly int MovementTriesMaxCap = 3;
    private static readonly int MovementTriesMinCap = 2;
    private static readonly int NewPathFindTriesMaxCap = 4;
    private static readonly int NewPathFindTriesMinCap = 2;

    private static readonly System.Random RandomNumberGenerator = new System.Random();

    public ActivityStateGather(MapCell Target) : base()
    {
        this.Target = Target;
        this.RefreshRemainingRetryCounts();
    }

    public override ActivityState SetCommands(Unit Unit, Skill Skill)
    {
        List<MapCell> Path2Resource = PathFinding.Instance.FindPath(Unit.CurrentCell, this.Target, PathFinding.EXCLUDE_LAST);

        this.CommandMove2Resource = new UnitCommandMove(this.Target, Path2Resource);

        this.CommandGatherFromResource = new UnitCommandGather(this.Target, Skill);
        this.CommandMove2Storage = null;
        this.CommandDrop2Storage = null;

        return this;
    }

    public override IEnumerator PerformAction(Unit Unit)
    {
        if (Unit.CurrentCommand.IsDone(Unit))
        {
            // If Unit arrived next to resource, let's command it to gather
            if (Unit.CurrentCommand == this.CommandMove2Resource)
            {
                Unit.CurrentCommand = this.CommandGatherFromResource;
            }
            // If Unit is finished gathering (full inventory), let's command it to move to storage
            else if (Unit.CurrentCommand == this.CommandGatherFromResource)
            {
                this.CommandToMoveResourcesToStorage(Unit);
                this.RefreshRemainingRetryCounts();
            }
            // If unit has walked next to the storage, let's command it to drop resources to it
            else if (Unit.CurrentCommand == this.CommandMove2Storage)
            {
                this.CommandDrop2Storage = new UnitCommandDrop(this.CommandMove2Storage.Target);
                Unit.CurrentCommand = this.CommandDrop2Storage;
            }
            // If unit has dropped resources to the storage, let's command it to move to the resource source again
            else if (Unit.CurrentCommand == this.CommandDrop2Storage)
            {
                // If target resource is depleted, there is no reason to move to it
                if (this.Target.CurrentObject == null || (this.Target.CurrentObject is ResourceSource && ((ResourceSource)this.Target.CurrentObject).Resources[0].IsDepleted()))
                {
                    Unit.SetActivity(new ActivityStateIdle());
                }
                // The target resource is not depleted, let's move to it
                else
                {
                    this.CommandMove2Resource = new UnitCommandMove(this.CommandMove2Resource.Target, PathFinding.Instance.FindPath(Unit.CurrentCell, this.CommandMove2Resource.Target, PathFinding.EXCLUDE_LAST));
                    Unit.CurrentCommand = this.CommandMove2Resource;
                    this.RefreshRemainingRetryCounts();
                }
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
            // If moving to resource is not possible
            if (Unit.CurrentCommand == this.CommandMove2Resource)
            {
                yield return Unit.StartCoroutine(UnableToMoveRoutine(Unit));
            }
            // If gathering from resource is not possible
            else if (Unit.CurrentCommand == this.CommandGatherFromResource)
            {
                Debug.Log("Gathering from this Resource Source is not possible because it is depleted");
                if (Unit.CarriedResource.IsDepleted())
                {
                    Unit.SetActivity(new ActivityStateIdle());
                }
                else
                {
                    this.CommandToMoveResourcesToStorage(Unit);
                }
            }
            // If moving to storage is not possible
            else if (Unit.CurrentCommand == this.CommandMove2Storage)
            {
                yield return Unit.StartCoroutine(UnableToMoveRoutine(Unit));
            }
            // If dropping resources at storage is not possible
            else if (Unit.CurrentCommand == this.CommandDrop2Storage)
            {
                if (MapControl.Instance.StorageList.Count > 0)
                {
                    this.CommandToMoveResourcesToStorage(Unit);
                }
                else
                {
                    Unit.SetActivity(new ActivityStateIdle());
                }
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
    public override void InitializeCommand(Unit Unit)
    {
        if (Unit.CarriedResource.IsDepleted())
        {
            Unit.CurrentCommand = this.CommandMove2Resource;
        }
        else
        {
            this.CommandToMoveResourcesToStorage(Unit);
        }
    }
    private void CommandToMoveResourcesToStorage(Unit Unit)
    {
        (List<MapCell>, MapCell) Temp = PathFinding.Instance.FindPath(Unit.CurrentCell, MapControl.Instance.StorageList, PathFinding.EXCLUDE_LAST);
        List<MapCell> Path = Temp.Item1;
        MapCell ClosestStorage = Temp.Item2;

        // Oh no, it's not possible to get to any Storage?
        if (Path == null)
        {
            Debug.Log("Neviem najst cestu nastavujem sa na idle!");
            Unit.SetActivity(new ActivityStateIdle());
        }
        // We found a path to Storage
        else
        {
            this.CommandMove2Storage = new UnitCommandMove(ClosestStorage, Path);
            Unit.CurrentCommand = this.CommandMove2Storage;
        }
    }
    private IEnumerator UnableToMoveRoutine(Unit Unit)
    {
        if (this.RemainingMovementTries <= 0)
        {
            if (this.RemainingNewPathFindTries <= 0)
            {
                Unit.SetActivity(new ActivityStateIdle());
            }
            else
            {
                this.RemainingNewPathFindTries--;
                ((UnitCommandMove)Unit.CurrentCommand).Targets.Clear();
                ((UnitCommandMove)Unit.CurrentCommand).Targets.AddRange(PathFinding.Instance.FindPath(Unit.CurrentCell, Unit.CurrentCommand.Target, PathFinding.EXCLUDE_LAST));
            }
        }
        else
        {
            this.RemainingMovementTries--;
            yield return Unit.StartCoroutine(Unit.WaitToRetryMove());
        }
    }
    private void RefreshRemainingRetryCounts()
    {
        this.RefreshRemainingMovementRetryCounts();
        this.RemainingNewPathFindTries = RandomNumberGenerator.Next(NewPathFindTriesMinCap, NewPathFindTriesMaxCap + 1);
    }
    private void RefreshRemainingMovementRetryCounts()
    {
        this.RemainingMovementTries = RandomNumberGenerator.Next(MovementTriesMinCap, MovementTriesMaxCap + 1);
    }
}
