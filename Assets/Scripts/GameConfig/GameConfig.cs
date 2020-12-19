using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newGameConfig", menuName = "Game Configuration")]
public class GameConfig : ScriptableObject
{
    [Header("Skills")]
    [Space(10)]
    [Tooltip("Defines the starting level of all skills. Beware: setting it higher doesn't automatically add any talents!")]
    [Min(1)]
    public int StartingLevelOfSkills = 1;
    [Min(1)]
    public int MaximumLevelOfSkills = 10;
    public int[] ExperienceToLevelUpForEachLevel = { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500 };
    [Min(0)]
    public int MaximumTalentsPerSkill = 3;
    [Tooltip("Defines levels at which unit recieves new talent. Ultimate talent is always recieved on the last level.")]
    public int[] RecieveTalentLevels = { 3, 6, 10 };


    [Header("Foraging skill")]
    [Min(0)]
    public int ForagingExperiencePerAction = 10;
    [Min(0)]
    public float ForagingGatheringTime = 1.5f;
    public Sprite ForagingIcon;
    public Sprite ForagingUnitSprite;
    public List<TalentSerializable> ForagingTalents;

    [Header("Mining skill")]
    [Min(0)]
    public int MiningExperiencePerAction = 10;
    [Min(0)]
    public float MiningGatheringTime = 3.0f;
    [Min(0)]
    public int BasicDiamondUnderRockChance = 25;
    public int MovementSpeedWhileCarryingRock = -50;
    public Sprite MiningIcon;
    public Sprite MiningUnitSprite;
    public List<TalentSerializable> MiningTalents;

    [Header("Hunting skill")]
    [Min(0)]
    public int HuntingExperiencePerAction = 10;
    [Min(0)]
    public int HuntingKillExperience = 30;
    [Min(0)]
    public float HuntingGatheringTime = 3.0f;
    public Sprite HuntingIcon;
    public Sprite HuntingUnitSprite;
    public List<TalentSerializable> HuntingTalents;

    [Header("Other")]
    [Space(10)]
    [Min(1)]
    public int MaxQueueActions = 20;
    public GameObject QueueFrame;
}