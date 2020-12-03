using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentPool : Singleton<TalentPool>
{
    private readonly Dictionary<SkillType, Dictionary<int, List<Talent>>> Talents;

    private TalentPool()
    {
        Talents = new Dictionary<SkillType, Dictionary<int, List<Talent>>>
        {
            { SkillType.Foraging, new Dictionary<int, List<Talent>>() },
            { SkillType.Hunting, new Dictionary<int, List<Talent>>() },
            { SkillType.Mining, new Dictionary<int, List<Talent>>() }
        };

        int i = 0;
        foreach (int level in GameConfigManager.Instance.GameConfig.RecieveTalentLevels)
        {
            Talents[SkillType.Foraging].Add(level, new List<Talent>());
            Talents[SkillType.Mining].Add(level, new List<Talent>());
            Talents[SkillType.Hunting].Add(level, new List<Talent>());

            if (level != GameConfigManager.Instance.GameConfig.RecieveTalentLevels.Last())
            {
                // foraging talents
                (string Name, string Description, List<EffectList> Effect, Sprite icon) = GameConfigManager.Instance.GameConfig.ForagingTalents[0].Unpack();
                Talents[SkillType.Foraging][level].Add(new TalentLumberjack(Name, Description, Effect[i], icon));

                (Name, Description, Effect, icon) = GameConfigManager.Instance.GameConfig.ForagingTalents[1].Unpack();
                Talents[SkillType.Foraging][level].Add(new TalentGatherer(Name, Description, Effect[i], icon));

                (Name, Description, Effect, icon) = GameConfigManager.Instance.GameConfig.ForagingTalents[2].Unpack();
                Talents[SkillType.Foraging][level].Add(new TalentForestFruits(Name, Description, Effect[i], icon));

                (Name, Description, Effect, icon) = GameConfigManager.Instance.GameConfig.ForagingTalents[3].Unpack();
                Talents[SkillType.Foraging][level].Add(new TalentFriendOfTheForest(Name, Description, Effect[i], icon));

                // mining talents
                (Name, Description, Effect, icon) = GameConfigManager.Instance.GameConfig.MiningTalents[0].Unpack();
                Talents[SkillType.Mining][level].Add(new TalentMiningBerserk(Name, Description, Effect[i], icon));

                (Name, Description, Effect, icon) = GameConfigManager.Instance.GameConfig.MiningTalents[1].Unpack();
                Talents[SkillType.Mining][level].Add(new TalentHeavyLifter(Name, Description, Effect[i], icon));

                (Name, Description, Effect, icon) = GameConfigManager.Instance.GameConfig.MiningTalents[2].Unpack();
                Talents[SkillType.Mining][level].Add(new TalentDualWielder(Name, Description, Effect[i], icon));

                //hunting talents
                (Name, Description, Effect, icon) = GameConfigManager.Instance.GameConfig.HuntingTalents[0].Unpack();
                Talents[SkillType.Hunting][level].Add(new TalentCarnivore(Name, Description, Effect[i], icon));

            }
            else
            {
                // foraging talents
                (string Name, string Description, List<EffectList> Effect, Sprite icon) = GameConfigManager.Instance.GameConfig.ForagingTalents[4].Unpack();
                Talents[SkillType.Foraging][level].Add(new TalentCriticalHarvest(Name, Description, Effect[0], icon));

                (Name, Description, Effect, icon) = GameConfigManager.Instance.GameConfig.ForagingTalents[5].Unpack();
                Talents[SkillType.Foraging][level].Add(new TalentMotherOfNature(Name, Description, icon));

                //mining talents
                (Name, Description, Effect, icon) = GameConfigManager.Instance.GameConfig.MiningTalents[3].Unpack();
                Talents[SkillType.Mining][level].Add(new TalentArcheologist(Name, Description, icon));
            }
            i++;
        }
    }

    public Talent RecieveNewTalent(List<Talent> UnitAppliedTalents, int level, SkillType type)
    {
        Talent t = null;
        if (this.Talents[type].ContainsKey(level))
        {
            t = GetNewTalent(UnitAppliedTalents, this.Talents[type][level]);
        }
        return t?.CreateNewInstanceOfSelf();
    }

    private Talent GetNewTalent(List<Talent> AppliedTalents, List<Talent> AllTalents)
    {
        // create empty talent pool
        List<Talent> TalentsFree = new List<Talent>();

        // iterate through all talents and those which aren't already applied add to the free pool
        if (AppliedTalents.Count > 0)
        {
            foreach (var talentAll in AllTalents)
            {
                bool isThere = false;
                foreach (var talentApplied in AppliedTalents)
                {
                    if (talentAll.GetType() == talentApplied.GetType())
                    {
                        // talent of this effect is already applied, skip
                        isThere = true;
                        break;
                    }
                }
                if (!isThere) 
                {
                    TalentsFree.Add(talentAll);
                }
            }
        }
        // all talents are available
        else
        {
            TalentsFree = AllTalents;
        }

        // generate random index (new talent)
        Talent ChoosenTalent = null;
        if (TalentsFree.Count() > 0)
        {
            ChoosenTalent = TalentsFree.ElementAt(UnityEngine.Random.Range(0, TalentsFree.Count()));
        }

        return ChoosenTalent;
    }

}
