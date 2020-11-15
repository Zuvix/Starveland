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
    private Unit UnitTarget;
    private Skill Skill;
    private int lastKnownX;
    private int lastKnownY;

    public ActivityStateHunt(Unit UnitTarget) : base()
    {
        this.UnitTarget = UnitTarget; 
    }

    public override ActivityState SetCommands(Unit Unit, Skill Skill = null)
    {
        List<MapCell> path = PathFinding.Instance.FindPath(Unit.CurrentCell, this.UnitTarget.CurrentCell);
        this.CommandMoveToTarget = new UnitCommandMove(this.UnitTarget.CurrentCell, path);
        this.CommandCombat = new UnitCommandCombatMelee(this.UnitTarget, Skill);
        this.lastKnownX = this.UnitTarget.CurrentCell.x;
        this.lastKnownY = this.UnitTarget.CurrentCell.y;
        this.Skill = Skill;

        return this;
    }

    public override void InitializeCommand(Unit Unit)
    {
        base.InitializeCommand(Unit);
        Unit.SetCommand(this.CommandCombat);
    }

    public override IEnumerator PerformAction(Unit Unit)
    {
        if (Unit.CurrentCommand.IsDone(Unit))
        {
            if (Unit.CurrentCommand == CommandMoveToTarget)
            {
                Unit.SetCommand(this.CommandCombat);
            }
            else if (Unit.CurrentCommand == CommandCombat)
            {
                Unit.SetActivity(new ActivityStateGather(MapControl.Instance.map.Grid[lastKnownX][lastKnownY]).SetCommands(
                    Unit, this.Skill));
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
                this.lastKnownX = this.UnitTarget.CurrentCell.x;
                this.lastKnownY = this.UnitTarget.CurrentCell.y;
                Unit.SetCommand(this.CommandMoveToTarget);
            }
        }
        else
        {
            yield return Unit.StartCoroutine(Unit.CurrentCommand.PerformAction(Unit));
        }
        
    }


}

