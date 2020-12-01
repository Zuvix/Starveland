using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillMining : Skill
{
    public int DiamondChance;
    public bool MiningBerserk;
    public int RemainingBerserkProcs;
    public int MiningBerserkChance;
    public int MiningBerserkTimes;
    public float MiningBerserkTimeSpan;
    public bool Archeologist;
    public bool DualWielder;
    public int ExtraMiningTargets;

    public SkillMining() : base()
    {
        this.ExperiencePerAction = GameConfigManager.Instance.GameConfig.MiningExperiencePerAction;
        this.GatheringTime = GameConfigManager.Instance.GameConfig.MiningGatheringTime;
        this.DiamondChance = GameConfigManager.Instance.GameConfig.BasicDiamondUnderRockChance;
        this.MovementSpeedModifier = GameConfigManager.Instance.GameConfig.MovementSpeedWhileCarryingRock;
        this.icon = GameConfigManager.Instance.GameConfig.MiningIcon;
        this.type = SkillType.Mining;
        this.MiningBerserk = false;
        this.RemainingBerserkProcs = 0;
        this.MiningBerserkChance = 0;
        this.MiningBerserkTimes = 0;
        this.MiningBerserkTimeSpan = 0f;
        this.Archeologist = false;
        this.DualWielder = false;
        this.ExtraMiningTargets = 0;
    }

    public override bool DoAction(Unit Unit, ResourceSource Target, out Resource Resource)
    {
        if (Target == null)
        {
            Resource = null;
            return false;
        }

        int x = Target.CurrentCell.x;
        int y = Target.CurrentCell.y;
        bool diamond = Target.tag.Equals("Diamond");

        // mining berserk talent
        if (this.MiningBerserk)
        {
            if (UnityEngine.Random.Range(1, 100) <= this.MiningBerserkChance)
            {
                this.RemainingBerserkProcs = this.MiningBerserkTimes;
            }
        }

        Resource = Target.GatherResource(1, out bool isDepleted);
        Unit.CarriedResource.AddDestructive(Resource);

        // dual wielder talent
        if (this.DualWielder)
        {
            int maximumTargets = this.ExtraMiningTargets;
            List<MapCell> neighbourFields = Target.CurrentCell.GetNeighbours();
            for (int i = 1; i < neighbourFields.Count; i++)
            {
                if (neighbourFields[i].GetTopSelectableObject() is ResourceSource)
                {
                    ResourceSource NeighbouringResourceSource = (ResourceSource)neighbourFields[i].GetTopSelectableObject();
                    if (NeighbouringResourceSource.tag.Equals("Stone") && maximumTargets-- > 0 && !Unit.InventoryFull())
                    {
                        int nx = NeighbouringResourceSource.CurrentCell.x;
                        int ny = NeighbouringResourceSource.CurrentCell.y;
                        Resource NeightbouringResource = NeighbouringResourceSource.GatherResource(1, out bool isDepletedNeighbouring);
                        NeighbouringResourceSource.Flash();
                        if (isDepletedNeighbouring && !diamond)
                        {
                            this.TargetDepleted(nx, ny);
                        }
                        Resource.Amount++;
                        Unit.CarriedResource.AddDestructive(NeightbouringResource);
                    }                    
                }
            }
        }

        if (isDepleted && !diamond)
        {
            this.TargetDepleted(x, y);
        }
      
        this.AddExperience(this.ExperiencePerAction, Unit);

        return true;
    }

    public override int GetMovementSpeedModifier(UnitPlayer Unit)
    {
        if (Unit.CarriedResource.itemInfo != null)
        {
            if (Unit.CarriedResource.itemInfo.isHeavy)
            {
                return this.MovementSpeedModifier;
            }
        }
        return 0;
    }

    public override float GetGatheringSpeed(ResourceSource resourceSource)
    {
        if (this.RemainingBerserkProcs-- > 0)
        {
            return this.MiningBerserkTimeSpan;
        }
        return this.GatheringTime;
    }

    private void TargetDepleted(int x, int y)
    {
        // basic chance to spawn diamond under rock
        if (UnityEngine.Random.Range(1, 100) <= this.DiamondChance)
        {
            ResourceSourceFactory.Instance.ProduceResourceSource(x, y, RSObjects.Diamond);
        }
        // archeologist talent
        else if (this.Archeologist)
        {
            ResourceSourceFactory.Instance.ProduceResourceSource(x, y, RSObjects.DeadAnimal);
        }
    }

}