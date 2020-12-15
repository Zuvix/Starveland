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
        this.ExperiencePerAction = GameConfigManager.Instance.GameConfig.HuntingExperiencePerAction;
        this.ExperiencePerKill = GameConfigManager.Instance.GameConfig.HuntingKillExperience;
        this.GatheringTime = GameConfigManager.Instance.GameConfig.HuntingGatheringTime;
        this.icon = GameConfigManager.Instance.GameConfig.HuntingIcon;
        this.unitSprite = GameConfigManager.Instance.GameConfig.HuntingUnitSprite;
        this.type = SkillType.Hunting;

        this.SkillTalents = new Dictionary<TalentType, Talent>()
        {
            { TalentType.Carnivore, null },
            { TalentType.LethalBlow, null },
            { TalentType.Opportunist, null },
            { TalentType.UnwaveringStance, null },
            { TalentType.ThroatSeeker, null },
            { TalentType.WindDancer, null },
            { TalentType.DivineBlessing, null },
            { TalentType.DeadEye, null }
        };
    }

    protected override void LevelUp(Unit Unit)
    {
        Unit.Defence += 5;
        if ((this.Level + 1) % 3 == 0) 
        {
            Unit.BaseDamage += 5;
        }
        base.LevelUp(Unit);
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
        bool lethal = false;
        if (SkillTalents[TalentType.LethalBlow] != null)
        {
            lethal = SkillTalents[TalentType.LethalBlow].Execute(Unit, TargetUnit);
        }
        if (!lethal)
        {
            Unit.Attack(Unit, TargetUnit, SkillTalents[TalentType.Opportunist] != null ? true : false);
        }

        //check if dead?
        if (TargetUnit.Health <= 0)
        {
            Debug.Log("Target killed! Getting experience!");
            this.AddExperience(this.ExperiencePerKill, Unit);
        }
        return true;
    }

    public override bool DoAction(Unit Unit, ResourceSource Target, out Resource Resource)
    {
        if (Target == null)
        {
            Resource = null;
            return false;
        }

        Resource = Target.GatherResource(1, out _);
        Unit.CarriedResource.AddDestructive(Resource);
        this.AddExperience(this.ExperiencePerAction, Unit);
        return true;
    }

    public override int GetExtraNutritionValue(Item item)
    {
        // Carnivore talent
        return SkillTalents[TalentType.Carnivore] == null ? 0 : SkillTalents[TalentType.Carnivore].Execute(item);
    }

}

