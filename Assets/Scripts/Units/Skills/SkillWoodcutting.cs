using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillWoodcutting : Skill
{
    public SkillWoodcutting() : base()
    {
        this.ExperiencePerAction = 10;
    }

    protected override bool LevelUp(Unit Unit)
    {
        //Console.WriteLine($"Woodcutting leveled up! (current level:{this.Level})");
        this.Level++;
        TalentSkillSpecific NewTalent = TalentPool.Instance.GetNewSkillSpecificTalent(this.SkillAppliedTalents, this.Level);
        if (NewTalent != null)
        {
            NewTalent.Apply(this);
            this.SkillAppliedTalents.Add(NewTalent);
            Debug.Log("Getting new talent: " + NewTalent.Name);
        }
        else
        {
            //Console.WriteLine("All possible talents are already active!");
        }
        return true; 
    }
}