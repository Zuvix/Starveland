using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TalentMovementSpeed : TalentUnitSpecific
{
    public float MovementSpeed;

    public TalentMovementSpeed(string Name, float MovementSpeed) : base(Name)
    {
        this.MovementSpeed = MovementSpeed;
    }

    public override bool Apply(Unit Unit)
    {
        Unit.MovementSpeed += this.MovementSpeed;
        return true;
    }

    public override bool Remove(Unit Unit)
    {
        Unit.MovementSpeed -= this.MovementSpeed;
        return true;
    }

    public override TalentUnitSpecific CreateNewInstanceOfSelf(int Level)
    {
        return new TalentMovementSpeed(this.Name, this.MovementSpeed * (float)(Level-1));
    }

}