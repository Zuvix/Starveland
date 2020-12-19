using System.Collections;
using UnityEngine;

public class UnitCommandCombatMelee : UnitCommand
{
    public Skill Skill;
    public Unit TargetUnit;

    private static readonly int CombatDistanceCap = 2;

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
        yield return Unit.StartCoroutine(Unit.Fight(this.TargetUnit));
        if (TargetUnit != null && Skill != null && PathFinding.Instance.BlockDistance(Unit.CurrentCell, TargetUnit.CurrentCell) <= CombatDistanceCap)
        {
            Unit.SetSprite(Skill.unitSprite);
            Skill.DoAction(Unit, TargetUnit);
        }
    }

    public override bool CanBePerformed(Unit Unit)
    {
        if (TargetUnit == null)
        {
            return false;
        }
        else if (TargetUnit.IsInBuilding())
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

