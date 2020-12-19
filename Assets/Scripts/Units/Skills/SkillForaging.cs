using System.Collections.Generic;

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

    public override bool DoAction(Unit Unit, ResourceSourceGeneric Target, out Resource Resource)
    {
        if (Target == null)
        {
            Resource = null;
            return false;
        }

        int x = Target.CurrentCell.x;
        int y = Target.CurrentCell.y;


        // Critical harvest talent
        Resource = SkillTalents[TalentType.CriticalHarvest] != null && Target is ResourceSource source1 ? SkillTalents[TalentType.CriticalHarvest].Execute(source1, out bool isDepleted) : Target.GatherResource(1, out isDepleted);

        if (Resource != null)
        {
            if (Target is ResourceSource source2)
            {
                // Gatherer talent
                Resource = this.SkillTalents[TalentType.Gatherer] != null ? this.SkillTalents[TalentType.Gatherer].Execute(Resource) : Resource;

                if (isDepleted)
                {
                    // Friend of the Forest talent
                    this.SkillTalents[TalentType.FriendOfTheForest]?.Execute(x, y, Resource);

                    // Mother of Nature talent, todo z foragable listu prefabov random vybrat, nenechat bush berry purple
                    this.SkillTalents[TalentType.MotherOfNature]?.Execute(x, y, Resource, source2);
                }
            }

            Unit.CarriedResource.AddDestructive(Resource);
            this.AddExperience(this.ExperiencePerAction, Unit);
        }

        return true;
    }

    public override int GetExtraNutritionValue(Item item)
    {
        // Forest Fruits talent
        return SkillTalents[TalentType.ForestFruits] == null ? 0 : SkillTalents[TalentType.ForestFruits].Execute(item);
    }

    public override float GetGatheringSpeed(ResourceSourceGeneric resourceSource)
    {
        if (resourceSource is ResourceSource resSource && resSource.resource.itemInfo.ItemType == ItemType.Material)
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