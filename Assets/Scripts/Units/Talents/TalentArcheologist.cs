using System;
using UnityEngine;

public class TalentArcheologist : Talent
{
    public TalentArcheologist(string Name, string Description, Sprite icon) : base(Name, Description, icon)
    {
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill is SkillMining)
        {
            ((SkillMining)(Skill)).Archeologist = true;
            return true;
        }
        else
        {
            throw new Exception("Talent archeologist tried to be applied to other skill than Mining!");
        }
    }


    public override bool Remove(Unit Unit, Skill Skill)
    {
        if (Skill is SkillMining)
        {
            ((SkillMining)(Skill)).Archeologist = false;
            return true;
        }
        else
        {
            throw new Exception("Talent archeologist tried to be removed from other skill than Mining!");
        }
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentArcheologist(this.Name, this.Description, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} ";
    }
}