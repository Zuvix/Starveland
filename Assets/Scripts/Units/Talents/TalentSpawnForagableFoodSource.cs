using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentSpawnForagableFoodSource : Talent
{
    public TalentSpawnForagableFoodSource(string Name, string Description, Sprite icon) : base(Name, Description, icon)
    {
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).MotherOfNatureActive = true;
            return true;
        }
        else
        {
            throw new Exception("Talent spawn foragable food source tried to be applied to other skill than Foraging!");
        }
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).MotherOfNatureActive = false;
            return true;
        }
        else
        {
            throw new Exception("Talent spawn foragable food source tried to be removed from other skill than Foraging!");
        }
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentSpawnForagableFoodSource(this.Name, this.Description, this.icon);
    }

    public override string Display()
    {
        return this.Description;
    }
}