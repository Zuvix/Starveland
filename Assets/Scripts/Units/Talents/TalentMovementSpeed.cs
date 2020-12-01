using UnityEngine;

public class TalentMovementSpeed : Talent
{
    public TalentMovementSpeed(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentEffects = Effects;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Unit.MovementSpeed *= Mathf.RoundToInt((((float)this.TalentEffects.Effects[0].effectValue / 100f) + 1f));
        return true;
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        Unit.MovementSpeed /= Mathf.RoundToInt((((float)this.TalentEffects.Effects[0].effectValue / 100f) + 1f));
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentMovementSpeed(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} {this.TalentEffects.Effects[0].effectValue}%";
    }
}