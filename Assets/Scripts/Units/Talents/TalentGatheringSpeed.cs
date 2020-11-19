using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TalentGatheringSpeed : TalentSkillSpecific
{
    public float GatheringSpeed;

    public TalentGatheringSpeed(string Name, float GatheringSpeed) : base(Name)
    {
        this.GatheringSpeed = GatheringSpeed;
    }

    public override bool Apply(Skill Skill)
    {
        Skill.GatheringTime -= this.GatheringSpeed;
        return true;
    }

    public override bool Remove(Skill Skill)
    {
        Skill.GatheringTime += this.GatheringSpeed;
        return true;
    }

    public override TalentSkillSpecific CreateNewInstanceOfSelf(int Level)
    {
        return new TalentGatheringSpeed(this.Name, this.GatheringSpeed * (float)(Level - 1));
    }
}