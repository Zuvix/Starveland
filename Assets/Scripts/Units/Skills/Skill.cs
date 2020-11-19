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

    // talents variables
    public float ChanceToGetExtraResource;
    public float GatheringTime;
    public int CarryingCapacity;

    public Skill()
    {
        this.ExperienceNeededToLevelUp = GameConfigManager.Instance.GameConfig.ExperienceNeededToLevelUp;
        this.CurrentExperience = 0;
        this.Level = 1;
        this.SkillAppliedTalents = new List<TalentSkillSpecific>();
        this.ChanceToGetExtraResource = 0;
        this.GatheringTime = 1.5f;
        this.CarryingCapacity = 2;
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