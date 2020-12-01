using UnityEngine;

public class TalentGatherFromMultipleRS : Talent
{
    public int slowedGatheringSpeed;
    public int extraTargets;

    public TalentGatherFromMultipleRS(string Name, string Description, EffectList Effects, Sprite icon) : base(Name, Description, icon)
    {
        this.TalentEffects = Effects;
        this.slowedGatheringSpeed = this.TalentEffects.Effects[0].effectValue;
        this.extraTargets = this.TalentEffects.Effects[1].effectValue;
    }

    public override bool Apply(Unit Unit, Skill Skill)
    {
        if (Skill is SkillMining)
        {
            Skill.GatheringTime *= (1f - ((float)this.slowedGatheringSpeed) / 100f);
            ((SkillMining)Skill).DualWielder = true;
            ((SkillMining)Skill).ExtraMiningTargets += this.extraTargets;
        }
        return true;
    }

    public override bool Remove(Unit Unit, Skill Skill)
    {
        if (Skill is SkillMining)
        {
            Skill.GatheringTime /= (1f - ((float)this.slowedGatheringSpeed) / 100f);
            ((SkillMining)Skill).DualWielder = false;
            ((SkillMining)Skill).ExtraMiningTargets -= this.extraTargets;
        }
        return true;
    }

    public override Talent CreateNewInstanceOfSelf()
    {
        return new TalentGatherFromMultipleRS(this.Name, this.Description, this.TalentEffects, this.icon);
    }

    public override string Display()
    {
        return $"{this.slowedGatheringSpeed}% {this.Description} +{this.extraTargets}";
    }
}