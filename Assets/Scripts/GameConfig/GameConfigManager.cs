using System;
using System.Linq;
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
    }
}
