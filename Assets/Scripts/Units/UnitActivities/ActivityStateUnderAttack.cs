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
    private Unit UnitTarget;

    public ActivityStateUnderAttack(Unit UnitTarget, Unit Unit) : base()
    {
        this.UnitTarget = UnitTarget;
        List<MapCell> path = PathFinding.Instance.FindPath(Unit.CurrentCell, this.UnitTarget.CurrentCell);
        this.CommandMoveToTarget = new UnitCommandMove(this.UnitTarget.CurrentCell, path);
        this.CommandCombat = new UnitCommandCombatMelee(this.UnitTarget);
    }

    public override void InitializeCommand(Unit Unit)
    {
        base.InitializeCommand(Unit);
        Unit.SetCommand(this.CommandCombat);
    }

    public override IEnumerator PerformSpecificAction(Unit Unit)
    {
        if (Unit is UnitAnimal && Unit.CurrentCommand == this.CommandMoveToTarget && DayCycleManager.Instance.GameIsWaitingForPlayerUnits2GoEat())
        {
            ((UnitAnimal)Unit).Wander();
        }
        else if (Unit.CurrentCommand.IsDone(Unit))
        {
            if (Unit.CurrentCommand == CommandMoveToTarget)
            {
                Unit.SetCommand(this.CommandCombat);
            }
            else if (Unit.CurrentCommand == CommandCombat)
            {
                if (Unit is UnitAnimal)
                {
                    ((UnitAnimal)Unit).Wander();
                }
                else
                {
                    Unit.SetActivity(new ActivityStateIdle());
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

