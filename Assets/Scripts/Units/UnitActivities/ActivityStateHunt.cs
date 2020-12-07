using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ActivityStateHunt : ActivityState
{
    private UnitCommandMove CommandMoveToTarget;
    private UnitCommandCombatMelee CommandCombat;
    private readonly Unit UnitTarget;
    private Skill Skill;

    public ActivityStateHunt(Unit UnitTarget) : base()
    {
        this.UnitTarget = UnitTarget; 
    }

    public override ActivityState SetCommands(Unit Unit, Skill Skill = null)
    {
        List<MapCell> path = PathFinding.Instance.FindPath(Unit.CurrentCell, this.UnitTarget.CurrentCell);
        this.CommandMoveToTarget = new UnitCommandMove(this.UnitTarget.CurrentCell, path);
        this.CommandCombat = new UnitCommandCombatMelee(this.UnitTarget, Skill);
        this.Skill = Skill;

        return this;
    }

    public override void InitializeCommand(Unit Unit)
    {
        base.InitializeCommand(Unit);
        Unit.SetCommand(this.CommandCombat);
    }

    public override IEnumerator PerformSpecificAction(Unit Unit)
    {
        if (Unit.CurrentCommand.IsDone(Unit))
        {
            if (Unit.CurrentCommand == CommandMoveToTarget)
            {
                Unit.SetCommand(this.CommandCombat);
            }
            else if (Unit.CurrentCommand == CommandCombat)
            {
                if (!DayCycleManager.Instance.GameIsWaitingForPlayerUnits2GoEat())
                {
                    if (Unit is UnitPlayer)
                    {
                        Unit.SetActivity(new ActivityStateGather(this.UnitTarget.CurrentCell).SetCommands(Unit, this.Skill));
                    }
                    else if (Unit is UnitAnimal)
                    {
                        ((UnitAnimal)Unit).Wander();
                    }
                }
                else
                {
                    if (Unit is UnitPlayer)
                    {
                        Unit.SetActivity(new ActivityStateEndDayRoutine());
                    }
                    else if (Unit is UnitAnimal)
                    {
                        ((UnitAnimal)Unit).Wander();
                    }
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

