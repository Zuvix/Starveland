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
    public int ExperienceNeededToLevelUp = 50;
    [Min(1)]
    public int StartingCarryingCapacityOfSkills = 2;
    [Min(0)]
    public float StartingGatheringTimeOfSkills = 1.5f;
    [Min(0)]
    public int StartingChanceToGetExtraResource = 5;

    [Header("Woodcutting skill")]
    [Min(0)]
    public int WoodcuttingExperiencePerAction = 10;
    public Sprite WoodcuttingIcon;

    [Header("Hunting skill")]
    [Min(0)]
    public int HuntingExperiencePerAction = 10;
    [Min(0)]
    public int HuntingKillExperience = 30;
    public Sprite HuntingIcon;

    [Header("Talents")]
    [Space(10)]
    [Min(0)]
    public int CarryingCapacityTalent = 200;
    public Sprite CarryingCapacityTalentIcon;
    [Tooltip("Should't be higher than 100/MaxNumberOfTalents, otherwise gathering speed can get to 0!")]
    [Min(0)]
    public int GatheringSpeedTalent = 30;
    public Sprite GatheringSpeedTalentIcon;
    [Min(0)]
    public int MovementSpeedTalent = 50;
    public Sprite MovementSpeedTalentIcon;

    [Header("Other")]
    [Space(10)]
    [Tooltip("Defines the chance of a unit moving to random position if he's wandering around and idling.")]
    [Range(0, 100)]
    public int ChanceToMoveDuringWandering = 10;
    [Tooltip("Defines how far a unit can move from its spawn position during wandering.")]
    [Min(0)]
    public int WanderingRadius = 2;

}

