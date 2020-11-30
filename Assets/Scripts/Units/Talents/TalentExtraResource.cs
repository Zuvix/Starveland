using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentExtraResource : Talent
{
    public TalentExtraResource(string Name, string Description, int ExtraResourceChance, Sprite icon, bool Ultimate) : base(Name, Description, icon, Ultimate)
    {
        this.Effect = ExtraResourceChance;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Skill.ChanceToGetExtraResource += this.Effect;
        return true;
    }


    public override bool Remove(Unit Unit, Skill Skill)
    {
        Skill.ChanceToGetExtraResource -= this.Effect;
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentExtraResource(this.Name, this.Description, this.Effect, this.icon, this.Ultimate);
    }
}

