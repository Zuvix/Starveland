using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentLethalBlow : Talent
{
    public int InstaKillChance;

    public TalentLethalBlow(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.LethalBlow;
        this.TalentEffects = Effects;
        this.InstaKillChance = this.TalentEffects.Effects[0].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentLethalBlow(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.Description} {this.InstaKillChance}%";
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        throw new NotImplementedException();
    }

    public override bool Execute(Unit Unit, Unit Target)
    {
        if (UnityEngine.Random.Range(1, 100) <= this.InstaKillChance)
        {
            Unit.CreatePopup("Lethal Blow!", Color.red);
            Target.Die();
            return true;
        }
        return false;
    }
}

