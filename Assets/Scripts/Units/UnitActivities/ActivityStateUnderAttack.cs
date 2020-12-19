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

    public ActivityStateUnderAttack(Unit UnitTarget, Unit Unit, Skill skill = null) : base()
    {
        this.UnitTarget = UnitTarget;
        List<MapCell> path = PathFinding.Instance.FindPath(Unit.CurrentCell, this.UnitTarget.CurrentCell);
        this.CommandMoveToTarget = new UnitCommandMove(this.UnitTarget.CurrentCell, path);
        this.CommandCombat = new UnitCommandCombatMelee(this.UnitTarget, skill);
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
                (this.UnitTarget.IsInBuilding())
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
                Unit.SetDefaultActivity();
            }
            else
            {
                Debug.LogError($"Unit's current command is done, but is something unexpected: {Unit.CurrentCommand}. Its current activity is: {Unit.CurrentActivity}");
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
                if (!UnitTarget.IsInBuilding())
                {
                    List<MapCell> path = PathFinding.Instance.FindPath(Unit.CurrentCell, this.UnitTarget.CurrentCell);
                    this.CommandMoveToTarget = new UnitCommandMove(this.UnitTarget.CurrentCell, path);
                    Unit.SetCommand(this.CommandMoveToTarget);
                }
                else
                {
                    Unit.SetDefaultActivity();
                }
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
    public override bool IsCancellable()
    {
        return true;
    }

    public override bool IsInterruptibleByAttack()
    {
        return false;
    }
}