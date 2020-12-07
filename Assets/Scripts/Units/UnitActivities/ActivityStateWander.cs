using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityStateWander : ActivityState
{
    private UnitCommandMove MoveCommand;
    private readonly UnitCommandIdle IdleCommand;
    private readonly int WanderingRadius;
    private readonly MapCell StartPosition;
    private readonly int chanceToMove;
    private readonly int AggroRadius;

    public ActivityStateWander(int WanderingRadius, MapCell StartPosition, int chanceToMove, int aggroRadius = 0) : base()
    {
        this.IdleCommand = new UnitCommandIdle();
        this.WanderingRadius = WanderingRadius;
        this.StartPosition = StartPosition;
        this.chanceToMove = chanceToMove;
        this.AggroRadius = aggroRadius;
    }

    public override void InitializeCommand(Unit Unit)
    {
        base.InitializeCommand(Unit);
        Unit.SetCommand(this.IdleCommand);
    }

    public override IEnumerator PerformSpecificAction(Unit Unit)
    {
        if (this.AggroRadius > 0)
        {
            UnitPlayer target = this.CheckEnemiesAround(Unit);
            if (target != null)
            {
                Unit.SetActivity(new ActivityStateHunt(target).SetCommands(Unit));
            }
        }
        if (Unit.CurrentCommand.IsDone(Unit))
        {
            if (Unit.CurrentCommand == this.IdleCommand)
            {
                this.MoveToRandomPosition(Unit);
                Unit.MovementConflictManager.RefreshRemainingRetryCounts();
            }
            else if (Unit.CurrentCommand == this.MoveCommand)
            {
                Unit.SetCommand(this.IdleCommand);
            }
            else
            {
                throw new Exception("Unit's current command is something unexpected");
            }
            this.PerformAction(Unit);
        }
        else if (!Unit.CurrentCommand.CanBePerformed(Unit))
        {
            if (Unit.CurrentCommand == this.MoveCommand)
            {
                yield return Unit.StartCoroutine(Unit.MovementConflictManager.UnableToMoveRoutine(Unit));
            }
        }
        else
        {
            if (Unit.CurrentCommand == this.IdleCommand)
            {
                if (UnityEngine.Random.Range(1, 100) <= this.chanceToMove)
                {
                    this.MoveToRandomPosition(Unit);
                }
            }
             yield return Unit.StartCoroutine(Unit.CurrentCommand.PerformAction(Unit));        
        }
    }

    private void MoveToRandomPosition(Unit Unit)
    {
        // generate random position to move on based on spawn and wandering radius
        int rx = Unit.CurrentCell.x, ry = Unit.CurrentCell.y;
        while (!MapControl.Instance.map.Grid[rx][ry].CanBeEnteredByUnit())
        {
            rx = UnityEngine.Random.Range(this.StartPosition.x - this.WanderingRadius, this.StartPosition.x + this.WanderingRadius);
            ry = UnityEngine.Random.Range(this.StartPosition.y - this.WanderingRadius, this.StartPosition.y + this.WanderingRadius);
            while (!MapControl.Instance.map.IsInBounds(rx, ry))
            {
                rx = UnityEngine.Random.Range(this.StartPosition.x - this.WanderingRadius, this.StartPosition.x + this.WanderingRadius);
                ry = UnityEngine.Random.Range(this.StartPosition.y - this.WanderingRadius, this.StartPosition.y + this.WanderingRadius);
            }
        }
     
        List<MapCell> path = PathFinding.Instance.FindPath(Unit.CurrentCell, MapControl.Instance.map.Grid[rx][ry]);

        if (path == null)
        {
            Unit.SetCommand(this.IdleCommand);
        }
        else
        {
            this.MoveCommand = new UnitCommandMove(MapControl.Instance.map.Grid[rx][ry], path);
            Unit.SetCommand(this.MoveCommand);
        }
    }

    private UnitPlayer CheckEnemiesAround(Unit Unit)
    {
        List<UnitPlayer> TargetList = new List<UnitPlayer>();
        foreach (UnitPlayer player in Unit.PlayerUnitPool)
        {
            //Debug.LogWarning(Vector2.Distance(new Vector2(Unit.CurrentCell.x, Unit.CurrentCell.y), new Vector2(player.CurrentCell.x, player.CurrentCell.y)));
            if (Vector2.Distance(new Vector2(Unit.CurrentCell.x, Unit.CurrentCell.y), new Vector2(player.CurrentCell.x, player.CurrentCell.y)) <= this.AggroRadius)
            {
                TargetList.Add(player);
            }
        }
        if (TargetList.Count > 0)
        {
            return TargetList[UnityEngine.Random.Range(0, TargetList.Count - 1)];
        }
        else
        {
            return null;
        }
    }

    public override bool IsCancellable()
    {
        return false;
    }
}

