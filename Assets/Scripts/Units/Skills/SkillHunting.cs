using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillHunting : Skill
{
    private int ExperiencePerKill;
    public SkillHunting() : base()
    {
        this.ExperienceNeededToLevelUp = GameConfigManager.Instance.GameConfig.HuntingExperienceNeededToLevelUp;
        this.ExperiencePerAction = GameConfigManager.Instance.GameConfig.HuntingExperiencePerAction;
        this.ExperiencePerKill = GameConfigManager.Instance.GameConfig.HuntingKillExperience;
        this.icon = GameConfigManager.Instance.GameConfig.HuntingIcon;
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
            Unit.CreatePopup(this.icon, $"Level Up!");
        }
        else
        {
            //Console.WriteLine("All possible talents are already active!");
        }
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
    
    private bool Attack(Unit Unit, Unit TargetUnit)
    {
        TargetUnit.DealDamage(Unit.BaseDamage, Unit);
        //Debug.Log("Dealing damage: " + Unit.BaseDamage);

        //check if dead?
        if (TargetUnit.Health <= 0)
        {
            Debug.Log("Target killed! Getting experience!");
            this.AddExperience(this.ExperiencePerKill, Unit);
        }

        return true;
    }

}

