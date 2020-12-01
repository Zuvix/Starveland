using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentSpawnSapling : Talent
{
    public TalentSpawnSapling(string Name, string Description, int SaplingSpawnChance, Sprite icon, bool Ultimate) : base(Name, Description, icon, Ultimate)
    {
        this.Effect = SaplingSpawnChance;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)(Skill)).ChanceToSpawnSapling += this.Effect;
            return true;
        }
        else
        {
            throw new Exception("Talent spawn sapling tried to be applied to other skill than Foraging!");
        }
    }


    public override bool Remove(Unit Unit, Skill Skill)
    {
        if (Skill is SkillForaging)
        {
            ((SkillForaging)(Skill)).ChanceToSpawnSapling -= this.Effect;
            return true;
        }
        else
        {
            throw new Exception("Talent spawn sapling tried to be removed from other skill than Foraging!");
        }
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentSpawnSapling(this.Name, this.Description, this.Effect, this.icon, this.Ultimate);
    }
}

