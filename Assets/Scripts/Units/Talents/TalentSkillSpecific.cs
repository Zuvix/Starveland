using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class TalentSkillSpecific : Talent
{

    public TalentSkillSpecific(string Name) :base(Name)
    {

    }

    public abstract bool Apply(Skill Skill);

    public abstract bool Remove(Skill Skill);

    public abstract TalentSkillSpecific CreateNewInstanceOfSelf(int Level);

}