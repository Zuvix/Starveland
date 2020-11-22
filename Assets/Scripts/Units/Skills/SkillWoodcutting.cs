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
        this.ExperienceNeededToLevelUp = GameConfigManager.Instance.GameConfig.WoodcuttingExperienceNeededToLevelUp;
        this.ExperiencePerAction = GameConfigManager.Instance.GameConfig.WoodcuttingExperiencePerAction;
        this.icon = GameConfigManager.Instance.GameConfig.WoodcuttingIcon;
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
            //Unit.CreatePopup(NewTalent.icon, $"New talent {NewTalent.Name}");
            Unit.CreatePopup(this.icon, $"Level Up!");
        }
        // all possible talents are already active, todo show a message or whatever..
        else
        {         
        }
        return true; 
    }
}