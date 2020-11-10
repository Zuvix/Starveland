using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SkillWoodcutting : Skill
{
    public SkillWoodcutting() : base()
    {
        this.ExperiencePerAction = 10;
    }

    protected override bool LevelUp()
    {
        Console.WriteLine($"Woodcutting leveled up! (current level:{this.Level})");
        this.Level++;
        TalentSkillSpecific NewTalent = TalentPool.Instance.GetNewSkillSpecificTalent(this.SkillAppliedTalents, this.Level);
        if (NewTalent != null)
        {
            NewTalent.Apply(this);
            this.SkillAppliedTalents.Add(NewTalent);
        }
        else
        {
            Console.WriteLine("All possible talents are already active!");
        }
        return true; 
    }

    public override bool DoAction(Unit Unit, ResourceSource Target)
    {
        this.Woodcut(Unit, Target);
        return true;
    }

    private bool Woodcut(Unit Unit, ResourceSource Target)
    {
        if (Target == null)
        {
            return false;
        }


        Unit.CarriedResource.AddDestructive(Target.GatherResource(1));
        // TODO mozno riesit ci naozaj bola surovina vytazena...? AddDestructive by mohla vraciat bool
        this.AddExperience(this.ExperiencePerAction);

        Console.WriteLine("I'm cutting wood {0}/{1}", Unit.CarriedResource.Amount, this.CarryingCapacity);

        return true;
    }
}