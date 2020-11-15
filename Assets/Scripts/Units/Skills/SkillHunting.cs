using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillHunting : Skill
{
    public SkillHunting() : base()
    {
        this.ExperiencePerAction = 10;
    }

    protected override bool LevelUp(Unit Unit) 
    {
        this.Level++;
        TalentUnitSpecific NewTalent = TalentPool.Instance.GetNewUnitSpecificTalent(((UnitPlayer)Unit).UnitAppliedTalents, this.Level);
        if (NewTalent != null)
        {
            NewTalent.Apply(Unit);
            ((UnitPlayer)Unit).UnitAppliedTalents.Add(NewTalent);
            Debug.Log("Unit getting new talent: " + NewTalent.Name);
        }
        else
        {
            //Console.WriteLine("All possible talents are already active!");
        }
        return true;
    }

    public override bool DoAction(Unit Unit, ResourceSource Target)
    {
        if (Target == null)
        {
            return false;
        }
        this.SkinDeadTarget(Unit, Target); //todo gather pre suroviny ktore vypadnu po zabiti nepriatela
        return true;
    }

    public override bool DoAction(Unit Unit, Unit TargetUnit)
    {
        if (TargetUnit == null)
        {
            return false;
        }
        this.Attack(Unit, TargetUnit);
        return true;
    }

    private bool SkinDeadTarget(Unit Unit, ResourceSource Target)
    {
        Unit.CarriedResource.AddDestructive(Target.GatherResource(1));
        this.AddExperience(this.ExperiencePerAction, Unit);
        return true;
    }
    
    private bool Attack(Unit Unit, Unit TargetUnit)
    {
        TargetUnit.DealDamage(Unit.BaseDamage, Unit);
        //Debug.Log("Dealing damage: " + Unit.BaseDamage);

        //check if dead?
        if (TargetUnit.Health <= 0)
        {
            Debug.Log("Target killed! Getting experience!");
            this.AddExperience(this.ExperiencePerAction * 3, Unit);
        }

        return true;
    }

}

