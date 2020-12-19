using System;
using UnityEngine;

public class TalentHeavyLifter : Talent
{
    public int ExtraInventorySize;

    public TalentHeavyLifter(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.HeavyLifter;
        this.TalentEffects = Effects;
        this.ExtraInventorySize = this.TalentEffects.Effects[0].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        ((UnitPlayer)Unit).CarryingCapacity += this.ExtraInventorySize;
        return true;
    }


    public override bool Remove(Unit Unit, Skill Skill)
    {
        ((UnitPlayer)Unit).CarryingCapacity -= this.ExtraInventorySize;
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentHeavyLifter(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return this.ExtraInventorySize > 0 ? $"{this.Description}, +{this.ExtraInventorySize}inv size" : $"{this.Description}";
    }

    public override int Execute(int value)
    {
        return Math.Max(0, value);
    }
}

