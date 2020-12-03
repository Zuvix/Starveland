using UnityEngine;

public class TalentCarnivore : Talent
{
    public int ExtraNutritionValue;
    public int BonusMaxHealth;
    private UnitPlayer Parent;

    public TalentCarnivore(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.Carnivore;
        this.TalentEffects = Effects;
        this.ExtraNutritionValue = this.TalentEffects.Effects[0].effectValue;
        this.BonusMaxHealth = this.TalentEffects.Effects[1].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        this.Parent = (UnitPlayer)Unit;
        return true;
    }
    public override bool Remove(Unit Unit, Skill Skill)
    {
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentCarnivore(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return this.BonusMaxHealth > 0 ? $"Meat has +{this.ExtraNutritionValue}nv, fully heals and add {this.BonusMaxHealth} max health" :
            $"Meat has +{this.ExtraNutritionValue}nv, fully heals";
    }

    public override int Execute(Item item)
    {
        if (item.foodType.Equals("Meat"))
        {
            this.Parent.MaxHealth += this.BonusMaxHealth;
            this.Parent.Health = this.Parent.MaxHealth;
            return this.ExtraNutritionValue;
        }
        return 0;
    }

}

