using UnityEngine;

public class TalentRemoveSlownessAndAddInventorySize : Talent
{
    public int ExtraInventorySize;
    private int originalEffect;

    public TalentRemoveSlownessAndAddInventorySize(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentEffects = Effects;
        this.ExtraInventorySize = this.TalentEffects.Effects[0].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill.MovementSpeedModifier < 0)
        {
            this.originalEffect = Skill.MovementSpeedModifier;
            Skill.MovementSpeedModifier = 0;
        }
        ((UnitPlayer)Unit).CarryingCapacity += this.ExtraInventorySize;
        return true;
    }


    public override bool Remove(Unit Unit, Skill Skill)
    {
        Skill.MovementSpeedModifier = this.originalEffect;
        ((UnitPlayer)Unit).CarryingCapacity -= this.ExtraInventorySize;
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentRemoveSlownessAndAddInventorySize(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return this.ExtraInventorySize > 0 ? $"{this.Description}, +{this.ExtraInventorySize}inv size" : $"{this.Description}";
    }
}

