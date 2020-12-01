using System;
using UnityEngine;

public class TalentWoodcuttingSpeed : Talent
{
    public TalentWoodcuttingSpeed(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        if (Effects.Effects[0].effectValue <= 100)
        {
            this.TalentEffects = Effects;
        }
        else
        {
            this.TalentEffects.Effects[0].effectValue = 100;
        }
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).WoodcuttingTime *= ((100f - (float)this.TalentEffects.Effects[0].effectValue) / 100f);
            return true;
        }
        else
        {
            throw new Exception("Talent woodcutting speed tried to be applied to other skill than Foraging!");
        }
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).WoodcuttingTime /= ((100f - (float)this.TalentEffects.Effects[0].effectValue) / 100f);
            return true;
        }
        else
        {
            throw new Exception("Talent woodcutting speed tried to be removed from other skill than Foraging!");
        }
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentWoodcuttingSpeed(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} {this.TalentEffects.Effects[0].effectValue}%";
    }
}