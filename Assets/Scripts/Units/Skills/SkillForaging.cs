using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillForaging : Skill
{
    private float WoodcuttingTime;
    private int WoodcuttingTimeIncrease;

    public SkillForaging() : base()
    {
        this.ExperiencePerAction = GameConfigManager.Instance.GameConfig.ForagingExperiencePerAction;
        this.GatheringTime = GameConfigManager.Instance.GameConfig.ForagingGatheringTime;
        this.icon = GameConfigManager.Instance.GameConfig.ForagingIcon;
        this.unitSprite = GameConfigManager.Instance.GameConfig.ForagingUnitSprite;
        this.type = SkillType.Foraging;
        this.WoodcuttingTime = this.GatheringTime;
        this.WoodcuttingTimeIncrease = 0;

        this.SkillTalents = new Dictionary<TalentType, Talent>()
        {
            { TalentType.Lumberjack, null },
            { TalentType.Gatherer, null },
            { TalentType.ForestFruits, null },
            { TalentType.FriendOfTheForest, null },
            { TalentType.MotherOfNature, null },
            { TalentType.CriticalHarvest, null }
        };
    }

    public override bool DoAction(Unit Unit, ResourceSource Target, out Resource Resource)
    {
        if (Target == null)
        {
            Resource = null;
            return false;
        }

        bool isDepleted;
        int x = Target.CurrentCell.x;
        int y = Target.CurrentCell.y;

        // Critical harvest talent
        Resource = SkillTalents[TalentType.CriticalHarvest] == null ? Target.GatherResource(1, out isDepleted) : 
            SkillTalents[TalentType.CriticalHarvest].Execute(Target, out isDepleted);

        // Gatherer talent
        Resource = this.SkillTalents[TalentType.Gatherer] != null ? this.SkillTalents[TalentType.Gatherer].Execute(Resource) : Resource;

        if (isDepleted)
        {
            // Friend of the Forest talent
            this.SkillTalents[TalentType.FriendOfTheForest]?.Execute(x, y, Resource);

            // Mother of Nature talent, todo z foragable listu prefabov random vybrat, nenechat bush berry purple
            this.SkillTalents[TalentType.MotherOfNature]?.Execute(x, y, Resource, Target);          
        }

        Unit.CarriedResource.AddDestructive(Resource);
        this.AddExperience(this.ExperiencePerAction, Unit);
        return true;
    }

    public override int GetExtraNutritionValue(Item item)
    {
        // Forest Fruits talent
        return SkillTalents[TalentType.ForestFruits] == null ? 0 : SkillTalents[TalentType.ForestFruits].Execute(item);
    }

    public override float GetGatheringSpeed(ResourceSource resourceSource)
    {
        if (resourceSource.resource.itemInfo.type.Equals("Resource"))
        {
            return SkillTalents[TalentType.CriticalHarvest] == null ? WoodcuttingTime : SkillTalents[TalentType.CriticalHarvest].Execute(WoodcuttingTime);
        }
        return this.GatheringTime;
    }

    public void IncreaseWoodcuttingSpeed(int value)
    {
        this.WoodcuttingTimeIncrease += value;
        this.WoodcuttingTime = this.GatheringTime / (this.GatheringTime * (float)(this.WoodcuttingTimeIncrease + 100f) / 100f);
    }
}