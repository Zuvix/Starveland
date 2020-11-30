using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentMovementSpeed : Talent
{
    public TalentMovementSpeed(string Name, string Description, int MovementSpeed, Sprite icon, bool Ultimate) : base(Name, Description, icon, Ultimate)
    {
        this.Effect = MovementSpeed;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Unit.MovementSpeed *= Mathf.RoundToInt((((float)this.Effect / 100f) + 1f));
        return true;
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        Unit.MovementSpeed /= Mathf.RoundToInt((((float)this.Effect / 100f) + 1f));
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentMovementSpeed(this.Name, this.Description, this.Effect, this.icon, this.Ultimate);
    }

}