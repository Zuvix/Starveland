using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillForaging : Skill
{
    public int NutritionValueBonus;
    public int ChanceToSpawnSapling;
    public bool CriticalHarvestActive;
    public bool MotherOfNatureActive;

    public SkillForaging() : base()
    {
        this.ExperiencePerAction = GameConfigManager.Instance.GameConfig.ForagingExperiencePerAction;
        this.GatheringTime = GameConfigManager.Instance.GameConfig.ForagingGatheringTime;
        this.icon = GameConfigManager.Instance.GameConfig.ForagingIcon;
        this.type = SkillType.Foraging;
        this.NutritionValueBonus = 0;
        this.ChanceToSpawnSapling = 0;
        this.CriticalHarvestActive = false;
        this.MotherOfNatureActive = false;
    }

    protected override bool LevelUp(Unit Unit)
    {
        this.Level++;
        Unit.CreatePopup(this.icon, $"Level Up!");
        Talent NewTalent = TalentPool.Instance.RecieveNewTalent(this.SkillAppliedTalents, this.Level, this.type);
        if (NewTalent != null)
        {
            NewTalent.Apply(Unit, this);
            this.SkillAppliedTalents.Add(NewTalent);
            Debug.Log("Getting new talent: " + NewTalent.Description);
            //Unit.CreatePopup(NewTalent.icon, $"New talent {NewTalent.Name}");
        }

        return true; 
    }

    public override bool DoAction(Unit Unit, ResourceSource Target, out Resource Resource)
    {
        if (Target == null)
        {
            Resource = null;
            return false;
        }
        Resource = Target.GatherResource(1);
        Unit.CarriedResource.AddDestructive(Resource);
        this.AddExperience(this.ExperiencePerAction, Unit);
        return true;
    }
}