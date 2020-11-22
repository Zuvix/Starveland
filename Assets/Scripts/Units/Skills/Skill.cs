using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Skill
{
    public int CurrentExperience { get; set; }
    public int Level;
    protected int ExperienceNeededToLevelUp;
    protected int ExperiencePerAction;
    public List<TalentSkillSpecific> SkillAppliedTalents;
    public Sprite icon;
    public bool Allowed { get; private set; }

    // talents variables
    public float ChanceToGetExtraResource;
    public float GatheringTime;
    public int CarryingCapacity;

    public Skill()
    {
        this.CurrentExperience = 0;
        this.Level = GameConfigManager.Instance.GameConfig.StartingLevelOfSkills;
        this.ExperienceNeededToLevelUp = GameConfigManager.Instance.GameConfig.ExperienceNeededToLevelUp;
        this.SkillAppliedTalents = new List<TalentSkillSpecific>();
        this.ChanceToGetExtraResource = GameConfigManager.Instance.GameConfig.StartingChanceToGetExtraResource;
        this.GatheringTime = GameConfigManager.Instance.GameConfig.StartingGatheringTimeOfSkills;
        this.CarryingCapacity = GameConfigManager.Instance.GameConfig.StartingCarryingCapacityOfSkills;
        this.Allowed = true;
    }

    protected bool AddExperience(int Amount, Unit Unit)
    {
        this.CurrentExperience += Amount;
        //if i have enough experience to level up
        if(CurrentExperience >= ExperienceNeededToLevelUp*Level)
        {
            this.LevelUp(Unit);
        }
        return true;
    }

    public void SetAllowed(bool value)
    {
        this.Allowed = value;
        UnitManager.Instance.ActionSchedulingLoop();
    }

    protected abstract bool LevelUp(Unit Unit);
    public bool DoAction(Unit Unit, ResourceSource Target, out Resource Resource)
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
    public virtual bool DoAction(Unit Unit, Unit TargetUnit)
    {
        return false;
    }

}