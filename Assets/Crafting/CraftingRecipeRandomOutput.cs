using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "newCraftingRecipeRandomOutput", menuName = "CraftingRecipeRandomOutput")]
public class CraftingRecipeRandomOutput : CraftingRecipe
{
    public List<(Resource, int)> Output;
    public string OutputScreenName;
    public Sprite Icon;
    private int ProbabilitySum = -1;

    public override (Sprite, int) ProduceOutput(BuildingCrafting ProducingBuilding)
    {
        /*GlobalInventory.Instance.AddItem(Output.Duplicate());
        return (Output.itemInfo.icon, Output.Amount);*/
        if (ProbabilitySum == -1)
        {
            ProbabilitySum = Output.Select(x => x.Item2).Sum();
        }
        int SelectedValue = Random.Range(0, ProbabilitySum);
        int SelectedOutputIndex;
        int Accumulator = 0;
        for (SelectedOutputIndex = 0; SelectedOutputIndex < Output.Count; SelectedOutputIndex++)
        {
            Accumulator += Output[SelectedOutputIndex].Item2;
            if (Accumulator >= SelectedValue)
            {
                break;
            }
        }
        SelectedOutputIndex = Mathf.Min(SelectedOutputIndex, Output.Count - 1);

        return (Output[SelectedOutputIndex].Item1.itemInfo.icon, Output[SelectedOutputIndex].Item1.Amount);
    }
    public override string OutputName()
    {
        return OutputScreenName;
    }
    protected override string CreateOutputDescription()
    {
        string Result = "";
        if (ProbabilitySum == -1)
        {
            ProbabilitySum = Output.Select(x => x.Item2).Sum();
        }
        foreach ((Resource, int) PotentialOutput in Output)
        {
            Result += $"{(int)(PotentialOutput.Item2 / ProbabilitySum * 100)}% chance for {PotentialOutput.Item1.itemInfo.name}\n";
        }
        if (Result.Length <= 0)
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
