using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillForaging : Skill
{
    public int ChanceToSpawnSapling;
    public bool CriticalHarvestActive;
    public bool MotherOfNatureActive;
    public float WoodcuttingTime;

    public SkillForaging() : base()
    {
        this.ExperiencePerAction = GameConfigManager.Instance.GameConfig.ForagingExperiencePerAction;
        this.GatheringTime = GameConfigManager.Instance.GameConfig.ForagingGatheringTime;
        this.icon = GameConfigManager.Instance.GameConfig.ForagingIcon;
        this.type = SkillType.Foraging;
        this.WoodcuttingTime = this.GatheringTime;
        this.ChanceToSpawnSapling = 0;
        this.CriticalHarvestActive = false;
        this.MotherOfNatureActive = false;
    }

    public override bool DoAction(Unit Unit, ResourceSource Target, out Resource Resource)
    {
        if (Target == null)
        {
            Resource = null;
            return false;
        }

        bool isForagable = Target.Resources[0].itemInfo.foodType.Equals("Foragable") ? true : false;
        bool isResource = Target.Resources[0].itemInfo.type.Equals("Resource") ? true : false;
        bool isDepleted;
        int x = Target.CurrentCell.x;
        int y = Target.CurrentCell.y;

        // Critical harvest talent
        if (CriticalHarvestActive && isResource)
        {
            Resource = Target.GatherResource(Target.Resources[0].Amount, out isDepleted);
        }
        else
        {
            Resource = Target.GatherResource(1, out isDepleted);
        }

        // Gatherer talent
        if (isForagable)
        {
            if (UnityEngine.Random.Range(1, 100) <= this.ChanceToGetExtraResource)
            {
                Resource.Amount += 1;
            }
        }

        if (isDepleted && isResource)
        {
            // Friend of the Forest talent
            if (UnityEngine.Random.Range(1, 100) <= this.ChanceToSpawnSapling)
            {
                ResourceSourceFactory.Instance.ProduceResourceSource(x, y, RSObjects.Sapling);
            }
            // Mother of Nature talent, todo z foragable listu prefabov random vybrat, nenechat bush berry purple
            if (this.MotherOfNatureActive)
            {
                List<MapCell> neighbouringFields = Target.CurrentCell.GetNeighbours();
                MapCell randomCell = neighbouringFields[UnityEngine.Random.Range(0, neighbouringFields.Count)];
                while (ResourceSourceFactory.Instance.ProduceResourceSource(randomCell.x, randomCell.y, RSObjects.Bush_Berry_Purple) == null)
                {
                    randomCell = neighbouringFields[UnityEngine.Random.Range(0, neighbouringFields.Count)];
                }
            }
        }

        Unit.CarriedResource.AddDestructive(Resource);
        this.AddExperience(this.ExperiencePerAction, Unit);
        return true;
    }

    public override int GetExtraNutritionValue(Item item)
    {
        // Forest Fruits talent
        if (item.foodType.Equals("Foragable"))
        {
            return this.ExtraNutritionValue;
        }
        return 0;
    }

    public override float GetGatheringSpeed(ResourceSource resourceSource)
    {
        // Lumberjack talent
        if (resourceSource.Resources[0].itemInfo.type.Equals("Resource")) 
        {
            return this.WoodcuttingTime;
        }
        else
        {
            return this.GatheringTime;
        }
    }
}