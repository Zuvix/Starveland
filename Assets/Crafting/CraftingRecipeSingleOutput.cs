using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCraftingRecipeSingleOutput", menuName = "CraftingRecipeSingleOutput")]
public class CraftingRecipeSingleOutput : CraftingRecipe
{
    public Resource Output;
    public override (Sprite, int) ProduceOutput(BuildingCrafting ProducingBuilding)
    {
        GlobalInventory.Instance.AddItem(Output.Duplicate());
        return (Output.itemInfo.icon, Output.Amount);
    }
    public override string OutputName()
    {
        return Output.itemInfo.name;
    }
    protected override string CreateOutputDescription()
    {
        return Output.itemInfo.NutritionValue == 0 ? "" : $"Eat for {Output.itemInfo.NutritionValue} nv";
    }
    public override Sprite OutputIcon()
    {
        return Output.itemInfo.icon;
    }
}
