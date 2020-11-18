using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Skill
{
    public int CurrentExperience { get; set; }
    public int Level;
    protected const int ExperienceNeededToLevelUp = 40; //TODO zmenit na 1000
    protected int ExperiencePerAction;
    public List<TalentSkillSpecific> SkillAppliedTalents;

    // talents variables
    public float ChanceToGetExtraResource;
    public float GatheringSpeed;
    public int CarryingCapacity;

    public Skill()
    {
        this.CurrentExperience = 0;
        this.Level = 1;
        this.SkillAppliedTalents = new List<TalentSkillSpecific>();
        this.ChanceToGetExtraResource = 0;
        this.GatheringSpeed = 1.5f;
        this.CarryingCapacity = 2;
    }

    protected bool AddExperience(int Amount, Unit Unit)
    {
        this.CurrentExperience += Amount;
        //Console.WriteLine($"Gained {Amount} experience, total {this.CurrentExperience}");

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