using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TalentSkillSpecific : Talent
{

    public TalentSkillSpecific(string Name) :base(Name)
    {

    }

    public virtual bool Apply(Skill Skill)
    {
        throw new NotImplementedException();
    }
    public virtual bool Remove(Skill Skill)
    {
        throw new NotImplementedException();
    }

    public virtual TalentSkillSpecific CreateNewInstanceOfSelf(int Level)
    {
        throw new NotImplementedException();
    }

}