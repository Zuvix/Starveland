using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCraftingRecipe", menuName = "CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<Resource> Input;
    public Resource Output;
    public bool IsUnlocked = true;
    public float CraftingDuration;
}
