using System.Collections.Generic;

public class SkillMining : Skill
{
    public int DiamondChance;
    private float MininigTime;
    private int MiningTimeIncrease;

    public SkillMining() : base()
    {
        this.ExperiencePerAction = GameConfigManager.Instance.GameConfig.MiningExperiencePerAction;
        this.GatheringTime = GameConfigManager.Instance.GameConfig.MiningGatheringTime;
        this.DiamondChance = GameConfigManager.Instance.GameConfig.BasicDiamondUnderRockChance;
        this.MovementSpeedModifier = GameConfigManager.Instance.GameConfig.MovementSpeedWhileCarryingRock;
        this.icon = GameConfigManager.Instance.GameConfig.MiningIcon;
        this.unitSprite = GameConfigManager.Instance.GameConfig.MiningUnitSprite;
        this.type = SkillType.Mining;
        this.MininigTime = this.GatheringTime;
        this.MiningTimeIncrease = 0;

        this.SkillTalents = new Dictionary<TalentType, Talent>()
        {
            { TalentType.Archeologist, null },
            { TalentType.MiningBerserk, null },
            { TalentType.HeavyLifter, null },
            { TalentType.DualWielder, null }
        };
    }

    public override bool DoAction(Unit Unit, ResourceSourceGeneric Target, out Resource Resource)
    {
        if (Target == null)
        {
            Resource = null;
            return false;
        }

        ResourceSource CastTarget = (ResourceSource)Target;

        int x = Target.CurrentCell.x;
        int y = Target.CurrentCell.y;

        // mining berserk talent
        this.SkillTalents[TalentType.MiningBerserk]?.Execute();

        Resource = Target.GatherResource(1, out bool isDepleted);
        Unit.CarriedResource.AddDestructive(Resource);

        // dual wielder talent
        this.SkillTalents[TalentType.DualWielder]?.Execute(Unit, CastTarget, Resource, this.TargetDepleted);
     
        if (isDepleted && !Target.CompareTag("Diamond"))
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

    public override float GetGatheringSpeed(ResourceSourceGeneric resourceSource)
    {
        return SkillTalents[TalentType.MiningBerserk] == null ? MininigTime : SkillTalents[TalentType.MiningBerserk].Execute(MininigTime);
    }

    private bool TargetDepleted(int x, int y)
    {
        // basic chance to spawn diamond under rock
        if (UnityEngine.Random.Range(1, 100) <= this.DiamondChance)
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

    public void IncreaseMiningSpeed(int valuePercent)
    {
        this.MiningTimeIncrease += valuePercent;
        this.MininigTime = this.GatheringTime / (this.GatheringTime * (float)(this.MiningTimeIncrease + 100f) / 100f);
    }
}