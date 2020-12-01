using System;
using UnityEngine;

public class TalentGatheringBurst : Talent
{
    public int BurstChance;
    public int NumberOfTimes;
    public float TimeSpan;

    public TalentGatheringBurst(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentEffects = Effects;
        this.BurstChance = this.TalentEffects.Effects[0].effectValue;
        this.NumberOfTimes = this.TalentEffects.Effects[1].effectValue;
        this.TimeSpan = (float)this.TalentEffects.Effects[2].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill is SkillMining)
        {
            ((SkillMining)Skill).MiningBerserk = true;
            ((SkillMining)Skill).MiningBerserkChance += this.BurstChance;
            ((SkillMining)Skill).MiningBerserkTimes += this.NumberOfTimes;
            ((SkillMining)Skill).MiningBerserkTimeSpan += this.TimeSpan / (float)this.NumberOfTimes;
            return true;
        }
        else
        {
            throw new Exception("Talent gathering burst tried to be applied to other skill than Mining! This is not implemented!");
        }
    }


    public override bool Remove(Unit Unit, Skill Skill)
    {
        if (Skill is SkillMining)
        {
            ((SkillMining)Skill).MiningBerserk = false;
            ((SkillMining)Skill).MiningBerserkChance -= this.BurstChance;
            ((SkillMining)Skill).MiningBerserkTimes -= this.NumberOfTimes;
            ((SkillMining)Skill).MiningBerserkTimeSpan -= this.TimeSpan / (float)this.NumberOfTimes;
            return true;
        }
        else
        {
            throw new Exception("Talent gathering burst tried to be removed from other skill than Mining! This is not implemented!");
        }
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentGatheringBurst(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.BurstChance}% chance to mine {this.NumberOfTimes}-times in {this.TimeSpan}s";
    }
}
