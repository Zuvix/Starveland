using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentGatheringSpeed : TalentSkillSpecific
{
    public TalentGatheringSpeed(string Name, int GatheringSpeed, Sprite icon) : base(Name, icon)
    {
        if (GatheringSpeed <= 100)
        {
            this.Effect = GatheringSpeed;
        }
        else
        {
            this.Effect = 100;
        }
    }

    public override bool Apply(Skill Skill)
    {
        Skill.GatheringTime *= ((100f - (float)this.Effect) / 100f);
        return true;
    }

    public override bool Remove(Skill Skill)
    {
        Skill.GatheringTime /= ((100f - (float)this.Effect) / 100f);
        return true;
    }

    public override TalentSkillSpecific CreateNewInstanceOfSelf(int Level)
    {
        return new TalentGatheringSpeed(this.Name, this.Effect * (Level - 1), this.icon);
    }
}