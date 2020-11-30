using UnityEngine;

public class TalentCarryingCapacity : Talent
{
    public TalentCarryingCapacity(string Name, string Description, int CarryingCapacity, Sprite icon, bool Ultimate) : base(Name, Description, icon, Ultimate)
    {
        this.Effect = CarryingCapacity;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Skill.CarryingCapacity *= Mathf.RoundToInt((((float)this.Effect / 100f) + 1f));
        return true;
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        Skill.CarryingCapacity /= Mathf.RoundToInt((((float)this.Effect / 100f) + 1f));
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentCarryingCapacity(this.Name, this.Description, this.Effect, this.icon, this.Ultimate);
    }

}