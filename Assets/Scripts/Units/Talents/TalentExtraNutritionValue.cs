using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentExtraNutritionValue : Talent
{
    public TalentExtraNutritionValue(string Name, string Description, int ExtraNutritionValue, Sprite icon, bool Ultimate) : base(Name, Description, icon, Ultimate)
    {
        this.Effect = ExtraNutritionValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        Skill.ExtraNutritionValue += this.Effect;
        return true;
    }


    public override bool Remove(Unit Unit, Skill Skill)
    {
        Skill.ExtraNutritionValue -= this.Effect;
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentExtraNutritionValue(this.Name, this.Description, this.Effect, this.icon, this.Ultimate);
    }

    public override string Display()
    {
        return $"{this.Description} +{this.Effect}nv";
    }
}

