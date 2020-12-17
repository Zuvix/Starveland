using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "newCraftingRecipeRandomOutput", menuName = "CraftingRecipeRandomOutput")]
public class CraftingRecipeRandomResourceOutput : CraftingRecipe
{
    [SerializeField]
    public List<RandomResourceOutputItem> Output;
    public string OutputScreenName;
    public Sprite Icon;

    public override (Sprite, int) ProduceOutput(BuildingCrafting ProducingBuilding)
    {
        (Sprite, int) Result;
        Resource SelectedResource = RandomItemChoice.SelectRandomOutputItem<Resource>(Output);
        
        if (SelectedResource == null)
        {
            Result = (PrefabPallette.Instance.VoidSprite, 0);
        }
        else
        {
            GlobalInventory.Instance.AddItem(SelectedResource.Duplicate());
            Result = (SelectedResource.itemInfo.icon, SelectedResource.Amount);
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
        foreach (RandomResourceOutputItem PotentialOutput in Output)
        {
            Result += $"{PotentialOutput.Probability}% chance for {PotentialOutput.OfferedResource.itemInfo.name}\n";
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
