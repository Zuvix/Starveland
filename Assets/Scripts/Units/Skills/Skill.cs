using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class Skill
{
    public int CurrentExperience { get; set; }
    public int Level;
    protected int[] ExperienceToLevelUpForEachLevel;
    protected int ExperiencePerAction;
    protected Dictionary<TalentType, Talent> SkillTalents;
    public List<Talent> AppliedTalents;
    public Sprite icon;
    public Sprite unitSprite;
    public bool Allowed { get; private set; }
    public SkillType type;
    public int MovementSpeedModifier;
    public float GatheringTime;
    public UnityEvent<UnitPlayer> onExperienceChanged = new UnityEvent<UnitPlayer>();

    private static readonly string LevelUpText = "Level Up!";
    private static readonly string TalentSuffixText = " talent!";
    public Skill()
    {
        this.CurrentExperience = 0;
        this.Level = GameConfigManager.Instance.GameConfig.StartingLevelOfSkills;
        this.ExperienceToLevelUpForEachLevel = GameConfigManager.Instance.GameConfig.ExperienceToLevelUpForEachLevel;
        this.AppliedTalents = new List<Talent>();
        this.Allowed = true;
        this.type = SkillType.none;
        this.MovementSpeedModifier = 0;
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
            this.onExperienceChanged.Invoke((UnitPlayer)Unit);
            return true;
        }
        return false;
    }

    protected virtual void  LevelUp(Unit Unit)
    {
        this.Level++;
        Talent NewTalent = TalentPool.Instance.RecieveNewTalent(this.AppliedTalents, this.Level, this.type);
        if (NewTalent != null)
        {
            NewTalent.Apply(Unit, this);
            this.AppliedTalents.Add(NewTalent);
            this.SkillTalents[NewTalent.TalentType] = NewTalent;
            Unit.CreatePopups(new List<(Sprite, string)>() { (this.icon, LevelUpText), (NewTalent.icon, NewTalent.Name + TalentSuffixText)});
        }
        else
        {
            Unit.CreatePopup(this.icon, LevelUpText);
        }
    }

    public void SetAllowed(bool value)
    {
        this.Allowed = value;
        UnitManager.Instance.ActionSchedulingLoop();
    }

    public virtual bool DoAction(Unit Unit, ResourceSourceGeneric Target, out Resource Resource)
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

    public virtual float GetGatheringSpeed(ResourceSourceGeneric resourceSource)
    {
        return this.GatheringTime;
    }

    public virtual int GetMovementSpeedModifier(UnitPlayer Unit)
    {
        return 0;
    }

}