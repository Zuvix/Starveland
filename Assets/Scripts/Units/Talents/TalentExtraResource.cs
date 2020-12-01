using UnityEngine;

public class TalentExtraResource : Talent
{
    public TalentExtraResource(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentEffects = Effects;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Skill.ChanceToGetExtraResource += this.TalentEffects.Effects[0].effectValue;
        return true;
    }


    public override bool Remove(Unit Unit, Skill Skill)
    {
        Skill.ChanceToGetExtraResource -= this.TalentEffects.Effects[0].effectValue;
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentExtraResource(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} {this.TalentEffects.Effects[0].effectValue}%";
    }
}

