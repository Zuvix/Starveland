using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentMovementSpeed : TalentUnitSpecific
{
    public TalentMovementSpeed(string Name, int MovementSpeed, Sprite icon) : base(Name, icon)
    {
        this.Effect = MovementSpeed;
    }

    public override bool Apply(Unit Unit)
    {
        Unit.MovementSpeed *= Mathf.RoundToInt((((float)this.Effect / 100f) + 1f));
        return true;
    }

    public override bool Remove(Unit Unit)
    {
        Unit.MovementSpeed /= Mathf.RoundToInt((((float)this.Effect / 100f) + 1f));
        return true;
    }

    public override TalentUnitSpecific CreateNewInstanceOfSelf(int Level)
    {
        return new TalentMovementSpeed(this.Name, this.Effect * (Level-1), this.icon);
    }

}