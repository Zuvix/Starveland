using UnityEngine;

public class TalentForestFruits : Talent
{
    public int ExtraNutritionValue;

    public TalentForestFruits(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.ForestFruits;
        this.TalentEffects = Effects;
        this.ExtraNutritionValue = this.TalentEffects.Effects[0].effectValue;
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
        return new TalentForestFruits(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} +{this.TalentEffects.Effects[0].effectValue}nv";
    }

    public override int Execute(Item item)
    {
        return item.foodType.Equals("Foragable") ? this.ExtraNutritionValue : 0;
    }
}