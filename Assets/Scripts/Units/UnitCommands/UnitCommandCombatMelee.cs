using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitCommandCombatMelee : UnitCommand
{
    public Skill Skill;
    public Unit TargetUnit;

    public UnitCommandCombatMelee(Unit TargetUnit, Skill Skill = null) : base(null)
    {
        this.TargetUnit = TargetUnit;
        this.Skill = Skill;
    }

    public override bool IsDone(Unit Unit)
    {
        return TargetUnit == null ? true : false;
    }

    public override IEnumerator PerformAction(Unit Unit)
    {
        yield return Unit.StartCoroutine(Unit.Fight(this.TargetUnit)); // todo send skill attack speed
        if (TargetUnit != null && Skill != null)
        {
            Skill.DoAction(Unit, TargetUnit);
        }
    }

    public override bool CanBePerformed(Unit Unit)
    {
        if (TargetUnit == null)
        {
            return false;
        }
        else if (Vector2.Distance(new Vector2(Unit.CurrentCell.x, Unit.CurrentCell.y), new Vector2(TargetUnit.CurrentCell.x, TargetUnit.CurrentCell.y)) > 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}

