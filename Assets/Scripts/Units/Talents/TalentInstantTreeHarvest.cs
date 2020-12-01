using System;
using UnityEngine;

public class TalentInstantTreeHarvest : Talent
{
    public TalentInstantTreeHarvest(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentEffects = Effects;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).CriticalHarvestActive = true;
            ((SkillForaging)Skill).WoodcuttingTime *= (float)this.TalentEffects.Effects[0].effectValue;
            return true;
        }
        else
        {
            throw new Exception("Talent instant tree harvest tried to be applied to other skill than Foraging!");
        }
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).CriticalHarvestActive = false;
            ((SkillForaging)Skill).WoodcuttingTime /= (float)this.TalentEffects.Effects[0].effectValue;
            return true;
        }
        else
        {
            throw new Exception("Talent instant critical harvest tried to be removed from other skill than Foraging!");
        }
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentInstantTreeHarvest(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} {this.TalentEffects.Effects[0].effectValue}x";
    }
}