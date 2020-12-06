using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "newCraftingRecipeRandomOutput", menuName = "CraftingRecipeRandomOutput")]
public class CraftingRecipeRandomOutput : CraftingRecipe
{
    [SerializeField]
    public List<RandomOutputItem> Output;
    public string OutputScreenName;
    public Sprite Icon;

    public override (Sprite, int) ProduceOutput(BuildingCrafting ProducingBuilding)
    {
        int SelectedValue = Random.Range(0, 100);
        int SelectedOutputIndex;
        int Accumulator = 0;
        for (SelectedOutputIndex = 0; SelectedOutputIndex < Output.Count; SelectedOutputIndex++)
        {
            Accumulator += Output[SelectedOutputIndex].Probability;
            if (Accumulator >= SelectedValue)
            {
                break;
            }
        }
        (Sprite, int) Result;
        if (SelectedOutputIndex >= Output.Count)
        {
            Result = (PrefabPallette.Instance.VoidSprite, 0);
        }
        else
        {
            GlobalInventory.Instance.AddItem(Output[SelectedOutputIndex].Item.Duplicate());
            Result = (Output[SelectedOutputIndex].Item.itemInfo.icon, Output[SelectedOutputIndex].Item.Amount);
        }
        

        return Result;
    }
    public override string OutputName()
    {
        return OutputScreenName;
    }
    protected override string CreateOutputDescription()
    {
        string Result = "";
        foreach (RandomOutputItem PotentialOutput in Output)
        {
            Result += $"{PotentialOutput.Probability}% chance for {PotentialOutput.Item.itemInfo.name}\n";
        }
        if (Result.Length > 0)
        {
            Result = Result.Remove(Result.Length - 1);
        }
        return Result;
    }
    public override Sprite OutputIcon()
    {
        return Icon;
    }
}
