using UnityEngine;

public class TalentCriticalHarvest : Talent
{
    public int WoodcuttingTimeSlowed;

    public TalentCriticalHarvest(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.CriticalHarvest;
        this.TalentEffects = Effects;
        this.WoodcuttingTimeSlowed = this.TalentEffects.Effects[0].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        return true;
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentCriticalHarvest(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} {this.TalentEffects.Effects[0].effectValue}x";
    }

    public override float Execute(float value)
    {
        return value * (float)this.WoodcuttingTimeSlowed;
    }

    public override Resource Execute(ResourceSource Target, out bool isDepleted)
    {
        return Target.GatherResource(Target.resource.itemInfo.ItemType == ItemType.Material ? Target.resource.Amount : 1, out isDepleted);
    }
}