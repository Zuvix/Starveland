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
    public int ChanceToGetExtraResource;
    protected int[] ExperienceToLevelUpForEachLevel;
    protected int ExperiencePerAction;
    public List<Talent> SkillAppliedTalents;
    public Sprite icon;
    public bool Allowed { get; private set; }
    public SkillType type;

    // talents variables
    public float GatheringTime;
    public int CarryingCapacity;

    public Skill()
    {
        this.CurrentExperience = 0;
        this.Level = GameConfigManager.Instance.GameConfig.StartingLevelOfSkills;
        this.ExperienceToLevelUpForEachLevel = GameConfigManager.Instance.GameConfig.ExperienceToLevelUpForEachLevel;
        this.SkillAppliedTalents = new List<Talent>();
        this.CarryingCapacity = GameConfigManager.Instance.GameConfig.StartingCarryingCapacityOfSkills;
        this.Allowed = true;
        this.ChanceToGetExtraResource = 0;
    }

    protected bool AddExperience(int Amount, Unit Unit)
    {
        this.CurrentExperience += Amount;
        //if i have enough experience to level up
        if(CurrentExperience >= this.ExperienceToLevelUpForEachLevel[this.Level - 1])
        {
            this.CurrentExperience = this.CurrentExperience - this.ExperienceToLevelUpForEachLevel[this.Level - 1];
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
    public virtual bool DoAction(Unit Unit, ResourceSource Target, out Resource Resource)
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