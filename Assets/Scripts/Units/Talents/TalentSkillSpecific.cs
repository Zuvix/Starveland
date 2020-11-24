using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class TalentSkillSpecific : Talent
{

    public TalentSkillSpecific(string Name, Sprite icon) : base(Name, icon)
    {

    }

    public abstract bool Apply(Skill Skill);

    public abstract bool Remove(Skill Skill);

    public abstract TalentSkillSpecific CreateNewInstanceOfSelf(int Level);

}