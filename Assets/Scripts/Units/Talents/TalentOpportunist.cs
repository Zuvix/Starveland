using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentOpportunist : Talent
{
    public TalentOpportunist(string Name, string Description, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.Opportunist;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentOpportunist(this.Name, this.Description, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description}";
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        return true;
    }

}

