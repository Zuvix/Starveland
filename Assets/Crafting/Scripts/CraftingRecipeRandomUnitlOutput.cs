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
        (Sprite, int) Result;
        GameObject SelectedUnit = RandomItemChoice.SelectRandomOutputItem<GameObject>(Output);

        if (SelectedUnit == null)
        {
            Result = (PrefabPallette.Instance.VoidSprite, 0);
        }
        else
        {
            MapCell SpawnMapCell = ProducingBuilding.CurrentCell.GetRandomUnitEnterableNeighbour();
            if (SpawnMapCell != null)
            {
                MapControl.Instance.CreateGameObject(SpawnMapCell.x, SpawnMapCell.y, SelectedUnit);
                Result = (SelectedUnit.GetComponent<SpriteRenderer>().sprite, 1);
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
