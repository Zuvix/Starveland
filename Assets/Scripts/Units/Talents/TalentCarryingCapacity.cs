using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentCarryingCapacity : TalentSkillSpecific
{
    public TalentCarryingCapacity(string Name, int CarryingCapacity, Sprite icon) : base(Name, icon)
    {
        this.Effect = CarryingCapacity;
    }

    public override bool Apply(Skill Skill)
    {
        Skill.CarryingCapacity *= Mathf.RoundToInt((((float)this.Effect / 100f) + 1f));
        return true;
    }

    public override bool Remove(Skill Skill)
    {
        Skill.CarryingCapacity /= Mathf.RoundToInt((((float)this.Effect / 100f) + 1f));
        return true;
    }

    public override TalentSkillSpecific CreateNewInstanceOfSelf(int Level)
    {
        return new TalentCarryingCapacity(this.Name, this.Effect * (Level-1), this.icon);
    }

}