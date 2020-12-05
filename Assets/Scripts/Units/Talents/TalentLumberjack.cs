using System;
using UnityEngine;

public class TalentLumberjack : Talent
{
    public int WoodcuttingBonus;

    public TalentLumberjack(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.Lumberjack;
        this.TalentEffects = Effects;
        this.WoodcuttingBonus = this.TalentEffects.Effects[0].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).IncreaseWoodcuttingSpeed(this.WoodcuttingBonus);
            return true;
        }
        else
        {
            throw new Exception($"Talent Lumberjack tried to be applied to {Skill.type}!");
        }
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        throw new NotImplementedException();
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentLumberjack(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} {this.TalentEffects.Effects[0].effectValue}%";
    }

    /*public override float Execute(ResourceSource resourceSource, float value)
    {
        if (resourceSource.Resources[0].itemInfo.type.Equals("Resource"))
        {
            return value * ((100f - (float)this.WoodcuttingBonus) / 100f);
        }
        return value;
    }*/
}