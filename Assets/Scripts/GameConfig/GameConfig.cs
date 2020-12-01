using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "newGameConfig", menuName = "Game Configuration")]
public class GameConfig : ScriptableObject
{
    [Header("Player Unit")]
    [Tooltip("Maximum health of the player units, default value is 100")]
    [Min(1)]
    public int MaxHealthPlayer = 100;
    [Range(0f, 50f)]
    public float MovementSpeedPlayer = 20f;
    [Min(1)]
    public int BaseDamagePlayer = 10;

    [Header("Animal Unit")]
    [Space(10)]
    [Min(1)]
    public int MaxHealthAnimal = 100;
    [Range(0f, 50f)]
    public float MovementSpeedAnimal = 5f;
    [Min(1)]
    public int BaseDamageAnimal = 10;

    [Header("Skills")]
    [Space(10)]
    [Tooltip("Defines the starting level of all skills. Beware: setting it higher doesn't automatically add any talents!")]
    [Min(1)]
    public int StartingLevelOfSkills = 1;
    [Min(1)]
    public int MaximumLevelOfSkills = 10;
    public int[] ExperienceToLevelUpForEachLevel = { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500 };
    [Min(0)]
    public float StartingGatheringTimeOfSkills = 1.5f;
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
    public List<TalentSerializable> ForagingTalents;

    [Header("Mining skill")]
    [Min(0)]
    public int MiningExperiencePerAction = 10;
    public Sprite MiningIcon;

    [Header("Hunting skill")]
    [Min(0)]
    public int HuntingExperiencePerAction = 10;
    [Min(0)]
    public int HuntingKillExperience = 30;
    public Sprite HuntingIcon;

    [Header("Other")]
    [Space(10)]
    [Tooltip("Defines the chance of a unit moving to random position if he's wandering around and idling.")]
    [Range(0, 100)]
    public int ChanceToMoveDuringWandering = 10;
    [Tooltip("Defines how far a unit can move from its spawn position during wandering.")]
    [Min(0)]
    public int WanderingRadius = 2;
    [Tooltip("Default size 28 is to fill full panel.")]
    [Min(1)]
    public int MaxQueueActions = 20;
    public GameObject QueueFrame;

}

