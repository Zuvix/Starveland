using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentThroatseeker : Talent
{
    public int TimesCritMulti;

    public TalentThroatseeker(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.ThroatSeeker;
        this.TalentEffects = Effects;
        this.TimesCritMulti = this.TalentEffects.Effects[0].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Unit.TimesCriticalMultiplier = this.TimesCritMulti;
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentThroatseeker(this.Name, this.Description, this.TalentEffects, this.icon); 
    }

    public override string Display()
    {
        return $"{this.TimesCritMulti}-times critical damage";
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        throw new NotImplementedException();
    }
}

