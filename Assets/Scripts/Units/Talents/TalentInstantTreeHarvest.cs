using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentInstantTreeHarvest : Talent
{
    public TalentInstantTreeHarvest(string Name, string Description, int WoodcuttingSpeedSlow, Sprite icon, bool Ultimate) : base(Name, Description, icon, Ultimate)
    {
        this.Effect = WoodcuttingSpeedSlow;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).CriticalHarvestActive = true;
            ((SkillForaging)Skill).WoodcuttingTime *= (float)this.Effect;
            return true;
        }
        else
        {
            throw new Exception("Talent instant tree harvest tried to be applied to other skill than Foraging!");
        }
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).CriticalHarvestActive = false;
            ((SkillForaging)Skill).WoodcuttingTime /= (float)this.Effect;
            return true;
        }
        else
        {
            throw new Exception("Talent instant critical harvest tried to be removed from other skill than Foraging!");
        }
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentInstantTreeHarvest(this.Name, this.Description, this.Effect, this.icon, this.Ultimate);
    }

    public override string Display()
    {
        return $"{this.Description} {this.Effect}x";
    }
}