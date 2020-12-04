using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillMining : Skill
{
    public int DiamondChance;

    public SkillMining() : base()
    {
        this.ExperiencePerAction = GameConfigManager.Instance.GameConfig.MiningExperiencePerAction;
        this.GatheringTime = GameConfigManager.Instance.GameConfig.MiningGatheringTime;
        this.DiamondChance = GameConfigManager.Instance.GameConfig.BasicDiamondUnderRockChance;
        this.MovementSpeedModifier = GameConfigManager.Instance.GameConfig.MovementSpeedWhileCarryingRock;
        this.icon = GameConfigManager.Instance.GameConfig.MiningIcon;
        this.type = SkillType.Mining;

        this.SkillTalents = new Dictionary<TalentType, Talent>()
        {
            { TalentType.Archeologist, null },
            { TalentType.MiningBerserk, null },
            { TalentType.HeavyLifter, null },
            { TalentType.DualWielder, null }
        };
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

        // mining berserk talent
        this.SkillTalents[TalentType.MiningBerserk]?.Execute();

        Resource = Target.GatherResource(1, out bool isDepleted);
        Unit.CarriedResource.AddDestructive(Resource);

        // dual wielder talent
        this.SkillTalents[TalentType.DualWielder]?.Execute(Unit, Target, Resource, this.TargetDepleted);
     
        if (isDepleted && !Target.tag.Equals("Diamond"))
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
                return SkillTalents[TalentType.HeavyLifter] == null ? MovementSpeedModifier : SkillTalents[TalentType.HeavyLifter].Execute(MovementSpeedModifier);
            }
        }
        return 0;
    }

    public override float GetGatheringSpeed(ResourceSource resourceSource)
    {
        return SkillTalents[TalentType.MiningBerserk] == null ? GatheringTime : SkillTalents[TalentType.MiningBerserk].Execute(GatheringTime);
    }

    private bool TargetDepleted(int x, int y)
    {
        // basic chance to spawn diamond under rock
        if (UnityEngine.Random.Range(1, 100) <= 1)
        {
            CellObjectFactory.Instance.ProduceResourceSource(x, y, RSObjects.Diamond);
        }
        // archeologist talent
        else 
        {
            this.SkillTalents[TalentType.Archeologist]?.Execute(x, y);
        }
        return true;
    }

}