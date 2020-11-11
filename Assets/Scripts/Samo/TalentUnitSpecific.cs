using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class TalentUnitSpecific : Talent
{

    public TalentUnitSpecific(string Name) : base(Name)
    {

    }

    public abstract bool Apply(Unit Unit);

    public abstract bool Remove(Unit Unit);

    public abstract TalentUnitSpecific CreateNewInstanceOfSelf(int Level);

}