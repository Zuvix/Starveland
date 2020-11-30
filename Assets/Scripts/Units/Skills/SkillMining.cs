using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillMining : Skill
{
    public SkillMining() : base()
    {
        this.ExperiencePerAction = GameConfigManager.Instance.GameConfig.MiningExperiencePerAction;
        this.icon = GameConfigManager.Instance.GameConfig.MiningIcon;
    }

    protected override bool LevelUp(Unit Unit)
    {
        this.Level++;
        Talent NewTalent = TalentPool.Instance.RecieveNewTalent(this.SkillAppliedTalents, this.Level, this.type);
        if (NewTalent != null)
        {
            NewTalent.Apply(Unit, this);
            this.SkillAppliedTalents.Add(NewTalent);
            Debug.Log("Getting new talent: " + NewTalent.Description);
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