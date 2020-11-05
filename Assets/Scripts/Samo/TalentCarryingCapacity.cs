using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TalentCarryingCapacity : TalentSkillSpecific
{
    public int CarryingCapacity;

    public TalentCarryingCapacity(string Name, int CarryingCapacity) : base(Name)
    {
        this.CarryingCapacity = CarryingCapacity;
    }

    public override bool Apply(Skill Skill)
    {
        Skill.CarryingCapacity += this.CarryingCapacity;
        return true;
    }

    public override bool Remove(Skill Skill)
    {
        Skill.CarryingCapacity -= this.CarryingCapacity;
        return true;
    }

    public override TalentSkillSpecific CreateNewInstanceOfSelf(int Level)
    {
        return new TalentCarryingCapacity(this.Name, this.CarryingCapacity*(Level-1));
    }

}