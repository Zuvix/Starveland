using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentGatheringSpeed : Talent
{
    public TalentGatheringSpeed(string Name, string Description, int GatheringSpeed, Sprite icon, bool Ultimate) : base(Name, Description, icon, Ultimate)
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

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Skill.GatheringTime *= ((100f - (float)this.Effect) / 100f);
        return true;
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        Skill.GatheringTime /= ((100f - (float)this.Effect) / 100f);
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentGatheringSpeed(this.Name, this.Description, this.Effect, this.icon, this.Ultimate);
    }
}