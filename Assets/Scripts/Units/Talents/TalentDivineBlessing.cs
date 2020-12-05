using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentDivineBlessing : Talent
{
    public int ChanceToProc;
    private Unit Parent;

    public TalentDivineBlessing(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.DivineBlessing;
        this.TalentEffects = Effects;
        this.ChanceToProc = this.TalentEffects.Effects[0].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        this.Parent = Unit;
        this.Parent.onDamageRecieved.AddListener(DivineBlessing);
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentDivineBlessing(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.ChanceToProc}{this.Description}";
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        this.Parent.onDamageRecieved.RemoveListener(DivineBlessing);
        return true;
    }

    private void DivineBlessing(int value)
    {
        if (this.Parent.Health <= 0)
        {
            if (UnityEngine.Random.Range(1,100) <= this.ChanceToProc)
            {
                this.Parent.Health = this.Parent.MaxHealth;
                this.Parent.CreatePopup(this.icon, "Divine Blessing!", Color.yellow);
            }
        }
    }

}

