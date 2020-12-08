using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newCraftingRecipeRandomUnitOutput", menuName = "CraftingRecipeRandomUnitOutput")]
public class CraftingRecipeRandomUnitlOutput : CraftingRecipe
{
    public List<RandomUnitOutputItem> Output;
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
            MapCell SpawnMapCell = ProducingBuilding.CurrentCell.GetRandomUnitEnterableNeighbour();
            if (SpawnMapCell != null)
            {
                MapControl.Instance.CreateGameObject(SpawnMapCell.x, SpawnMapCell.y, Output[SelectedOutputIndex].OfferedUnit);
                Result = (Output[SelectedOutputIndex].OfferedUnit.GetComponent<SpriteRenderer>().sprite, 1);
            }
            else
            {
                Result = (PrefabPallette.Instance.VoidSprite, 0);
            }
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
        foreach (RandomUnitOutputItem PotentialOutput in Output)
        {
            Result += $"{PotentialOutput.Probability}% chance for {PotentialOutput.OfferedUnit.GetComponent<CellObject>().objectName}\n";
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
