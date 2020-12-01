using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigManager : Singleton<GameConfigManager>
{
    public GameConfig GameConfig;

    private void Awake()
    {
        ValidateSkillTalentConfig();
    }

    private void ValidateSkillTalentConfig()
    {
        if (GameConfig.RecieveTalentLevels.Last() != GameConfig.MaximumLevelOfSkills)
        {
            throw new Exception("Last recieve talent level should be equal to maximum level of skills! The talent system may not work properly!");
        }
        if (GameConfig.RecieveTalentLevels.Length != GameConfig.MaximumTalentsPerSkill)
        {
            throw new Exception("Number of recieve talent levels should be equal to maximum talents per skill! The talent system may not work properly!");
        }
        if (!(GameConfig.RecieveTalentLevels.Distinct().Count() == GameConfig.RecieveTalentLevels.Count()))
        {
            throw new Exception("Values in recieve talent levels should be unique! The talent system may not work properly!");
        }
        if (GameConfig.ExperienceToLevelUpForEachLevel.Length != GameConfig.MaximumLevelOfSkills)
        {
            throw new Exception("Experience to level up for each level is not correctly defined! There should by experience for each level!");
        }
        //check foraging talents
        foreach (var talent in this.GameConfig.ForagingTalents)
        {
            if (!talent.Ultimate)
            {
                if (talent.Effect.Length != GameConfig.MaximumTalentsPerSkill - 1)
                {
                    throw new Exception($"Foraging talent {talent.Name} has incorrectly defined effects! " +
                        $"There should be effect for each recieve talent level except last! " +
                        $"The talent system may not work properly because of this!");
                }
            }
            else
            {
                /*if (talent.Effect.Length > 0)
                {
                    throw new Exception($"Foraging talent {talent.Name} is defined as ultimate and therefore it doesn't need any effects!");
                }*/
            }
        }

    }
}
