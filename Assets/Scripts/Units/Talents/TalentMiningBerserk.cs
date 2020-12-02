using System;
using UnityEngine;

public class TalentMiningBerserk : Talent
{
    public int BurstChance;
    public int NumberOfTimes;
    public float TimeSpan;
    private int RemainingBerserkProcs;
    private readonly float MiningBeserkTimeSpan;

    public TalentMiningBerserk(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentType = TalentType.MiningBerserk;
        this.TalentEffects = Effects;
        this.BurstChance = this.TalentEffects.Effects[0].effectValue;
        this.NumberOfTimes = this.TalentEffects.Effects[1].effectValue;
        this.TimeSpan = (float)this.TalentEffects.Effects[2].effectValue;
        this.RemainingBerserkProcs = 0;
        this.MiningBeserkTimeSpan = this.TimeSpan / (float)this.NumberOfTimes;
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
        return new TalentMiningBerserk(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.BurstChance}% chance to mine {this.NumberOfTimes}-times in {this.TimeSpan}s";
    }

    public override void Execute()
    {
        if (UnityEngine.Random.Range(1, 100) <= this.BurstChance)
        {
            this.RemainingBerserkProcs = this.NumberOfTimes;
        }  
        else
        {
            this.RemainingBerserkProcs = 0;
        }
    }

    public override float Execute(float value)
    {
        if (this.RemainingBerserkProcs-- > 0)
        {
            return this.MiningBeserkTimeSpan;
        }
        return value;
    }
}
