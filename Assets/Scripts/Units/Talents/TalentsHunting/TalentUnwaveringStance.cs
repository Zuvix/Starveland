using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentUnwaveringStance : Talent
{
    public TalentUnwaveringStance(string Name, string Description, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.UnwaveringStance;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Unit.Defence *= 2;
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentUnwaveringStance(this.Name, this.Description, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description}";
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        Unit.Defence /= 2;
        return true;
    }

}

