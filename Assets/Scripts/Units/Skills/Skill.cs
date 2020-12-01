﻿using System;
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
    public int ExtraNutritionValue;
    protected int[] ExperienceToLevelUpForEachLevel;
    protected int ExperiencePerAction;
    public List<Talent> AppliedTalents;
    public Sprite icon;
    public bool Allowed { get; private set; }
    public SkillType type;

    public float GatheringTime;

    public Skill()
    {
        this.CurrentExperience = 0;
        this.Level = GameConfigManager.Instance.GameConfig.StartingLevelOfSkills;
        this.ExperienceToLevelUpForEachLevel = GameConfigManager.Instance.GameConfig.ExperienceToLevelUpForEachLevel;
        this.GatheringTime = GameConfigManager.Instance.GameConfig.StartingGatheringTimeOfSkills;
        this.AppliedTalents = new List<Talent>();
        this.Allowed = true;
        this.ChanceToGetExtraResource = 0;
        this.ExtraNutritionValue = 0;
        this.type = SkillType.none;
    }

    protected bool AddExperience(int Amount, Unit Unit)
    {
        if (this.Level < GameConfigManager.Instance.GameConfig.MaximumLevelOfSkills)
        {
            this.CurrentExperience += Amount;
            //if i have enough experience to level up
            if (CurrentExperience >= this.ExperienceToLevelUpForEachLevel[this.Level - 1])
            {
                this.CurrentExperience -= this.ExperienceToLevelUpForEachLevel[this.Level - 1];
                this.LevelUp(Unit);
            }
            return true;
        }
        return false;
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

        Resource = Target.GatherResource(1, out bool isDepleted);
        Unit.CarriedResource.AddDestructive(Resource);
        this.AddExperience(this.ExperiencePerAction, Unit);
        return true;
    }
    public virtual bool DoAction(Unit Unit, Unit TargetUnit)
    {
        return false;
    }

    public virtual int GetExtraNutritionValue(Item item)
    {
        return 0;
    }

    public virtual float GetGatheringSpeed(ResourceSource resourceSource)
    {
        return this.GatheringTime;
    }

}