using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentWoodcuttingSpeed : Talent
{
    public TalentWoodcuttingSpeed(string Name, string Description, int WoodcuttingSpeed, Sprite icon, bool Ultimate) : base(Name, Description, icon, Ultimate)
    {
        if (WoodcuttingSpeed <= 100)
        {
            this.Effect = WoodcuttingSpeed;
        }
        else
        {
            this.Effect = 100;
        }
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).WoodcuttingTime *= ((100f - (float)this.Effect) / 100f);
            return true;
        }
        else
        {
            throw new Exception("Talent woodcutting speed tried to be applied to other skill than Foraging!");
        }
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)Skill).WoodcuttingTime /= ((100f - (float)this.Effect) / 100f);
            return true;
        }
        else
        {
            throw new Exception("Talent woodcutting speed tried to be removed from other skill than Foraging!");
        }
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentWoodcuttingSpeed(this.Name, this.Description, this.Effect, this.icon, this.Ultimate);
    }
}