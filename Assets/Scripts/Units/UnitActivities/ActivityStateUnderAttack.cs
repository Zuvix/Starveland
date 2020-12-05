using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ActivityStateUnderAttack : ActivityState
{
    private UnitCommandMove CommandMoveToTarget;
    private UnitCommandCombatMelee CommandCombat;
    public Unit UnitTarget { get; private set; }
    private readonly int originalX;
    private readonly int originalY;
    private readonly int waderingRadius;
    private readonly int chanceToMove;

    public ActivityStateUnderAttack(Unit UnitTarget, Unit Unit, int originalX = -1, int originalY = -1, int wanderingRadius = 2, int chanceToMove = 0) : base()
    {
        this.UnitTarget = UnitTarget;
        List<MapCell> path = PathFinding.Instance.FindPath(Unit.CurrentCell, this.UnitTarget.CurrentCell);
        this.CommandMoveToTarget = new UnitCommandMove(this.UnitTarget.CurrentCell, path);
        this.CommandCombat = new UnitCommandCombatMelee(this.UnitTarget);
        this.originalX = originalX;
        this.originalY = originalY;
        this.waderingRadius = wanderingRadius;
        this.chanceToMove = chanceToMove;
    }

    public override void InitializeCommand(Unit Unit)
    {
        Unit.SetCommand(this.CommandCombat);
    }

    public override IEnumerator PerformSpecificAction(Unit Unit)
    {
        if (
                (Unit is UnitAnimal && Unit.CurrentCommand == this.CommandMoveToTarget && DayCycleManager.Instance.TimeOut)
                ||
                (Unit.CurrentCommand == CommandMoveToTarget && PathFinding.Instance.BlockDistance(Unit.CurrentCell, UnitTarget.CurrentCell) > Unit.TargetDistance2AbortAttackOn)
            )
        {
            Unit.SetDefaultActivity();
        }
        else if (Unit.CurrentCommand.IsDone(Unit))
        {
            if (Unit.CurrentCommand == CommandMoveToTarget)
            {
                Unit.SetCommand(this.CommandCombat);
            }
            else if (Unit.CurrentCommand == CommandCombat)
            {
                if (this.originalX == -1 || this.originalY == -1)
                {
                    Unit.SetActivity(new ActivityStateIdle());
                }
                else
                {
                    Unit.SetActivity(new ActivityStateWander(this.waderingRadius, MapControl.Instance.map.Grid[originalX][originalY], this.chanceToMove));
                }
            }
        }
        else if (!Unit.CurrentCommand.CanBePerformed(Unit))
        {
            if (Unit.CurrentCommand == this.CommandMoveToTarget)
            {
                yield return Unit.StartCoroutine(Unit.MovementConflictManager.UnableToMoveRoutine(Unit));
            }
            else if (Unit.CurrentCommand == this.CommandCombat)
            {
                List<MapCell> path = PathFinding.Instance.FindPath(Unit.CurrentCell, this.UnitTarget.CurrentCell);
                this.CommandMoveToTarget = new UnitCommandMove(this.UnitTarget.CurrentCell, path);
                Unit.SetCommand(this.CommandMoveToTarget);
            }
        }
        else
        {
            yield return Unit.StartCoroutine(Unit.CurrentCommand.PerformAction(Unit));
        }
    }
    public override bool IsCancellable()
    {
        return true;
    }
}

