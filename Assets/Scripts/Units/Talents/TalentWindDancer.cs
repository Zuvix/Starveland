using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentWindDancer : Talent
{
    public int ExtraDodge;

    public TalentWindDancer(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.WindDancer;
        this.TalentEffects = Effects;
        this.ExtraDodge = this.TalentEffects.Effects[0].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Unit.Dodge += this.ExtraDodge;
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentWindDancer(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} +{this.ExtraDodge}%";
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        Unit.Dodge -= this.ExtraDodge;
        return true;
    }

}

