using UnityEngine;

public class TalentGatherer : Talent
{
    public int ExtraForagableFoodChance;

    public TalentGatherer(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.Gatherer;
        this.TalentEffects = Effects;
        this.ExtraForagableFoodChance = this.TalentEffects.Effects[0].effectValue;
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
        return new TalentGatherer(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} {this.TalentEffects.Effects[0].effectValue}%";
    }

    public override Resource Execute(Resource Resource)
    {
        if (Resource.itemInfo.FoodType == FoodType.Foragable && Random.Range(1, 100) <= this.ExtraForagableFoodChance)
        {
            Resource.Amount++;
        }
        return Resource;
    }
}

