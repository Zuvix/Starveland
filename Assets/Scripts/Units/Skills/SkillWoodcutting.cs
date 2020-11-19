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

        //this.ExperienceNeededToLevelUp = GameConfigManager.Instance.GameConfig.ExperienceNeededToLevelUp;
        //this.ExperienceNeededToLevelUp = 50;
        /*this.CurrentExperience = 0;
        this.Level = 1;
        this.SkillAppliedTalents = new List<TalentSkillSpecific>();
        this.ChanceToGetExtraResource = 0;
        this.GatheringTime = 1.5f;
        this.CarryingCapacity = 2;*/
    }

    protected override bool LevelUp(Unit Unit)
    {
        this.Level++;
        TalentSkillSpecific NewTalent = TalentPool.Instance.GetNewSkillSpecificTalent(this.SkillAppliedTalents, this.Level);
        if (NewTalent != null)
        {
            NewTalent.Apply(this);
            this.SkillAppliedTalents.Add(NewTalent);
            Debug.Log("Getting new talent: " + NewTalent.Name);
        }
        // all possible talents are already active, todo show a message or whatever..
        else
        {         
        }
        return true; 
    }
}