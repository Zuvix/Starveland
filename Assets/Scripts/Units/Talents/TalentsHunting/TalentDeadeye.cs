using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentDeadeye : Talent
{
    public int ExtraCritChance;
    public int ExtraAccuracy;

    public TalentDeadeye(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.DeadEye;
        this.TalentEffects = Effects;
        this.ExtraAccuracy = this.TalentEffects.Effects[0].effectValue;
        this.ExtraCritChance = this.TalentEffects.Effects[1].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Unit.CritChance += this.ExtraCritChance;
        Unit.Accuracy += this.ExtraAccuracy;
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentDeadeye(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"+{this.ExtraAccuracy}% accuracy, +{this.ExtraCritChance}% crit chance";
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        Unit.CritChance -= this.ExtraCritChance;
        Unit.Accuracy -= this.ExtraAccuracy;
        return true;
    }

}

