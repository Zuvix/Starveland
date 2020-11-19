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
    [Tooltip("This value defines the starting level of all skills. Beware: setting it higher doesn't automatically add any talents!")]
    [Min(0)]
    public int StartingLevelOfSkills = 1;
    [Header("Woodcutting skill")]
    [Min(1)]
    public int ExperienceNeededToLevelUp = 50;

    [Header("Talents")] //todo prerobit na percenta
    [Space(10)]
    public int CarryingCapacityTalent = 5;

    [Header("Other")]
    [Space(10)]
    [Tooltip("This value defines the chance of unit moving to random position if he's wandering around and idling.")]
    [Range(0, 100)]
    public int ChanceToMoveDuringWandering = 10;
    [Tooltip("This value defines how far a unit can move from its spawn position during wandering.")]
    [Min(0)]
    public int WanderingRadius = 2;

}

