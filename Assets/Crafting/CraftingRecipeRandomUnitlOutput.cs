using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeRandomUnitlOutput : CraftingRecipe
{
    public List<RandomResourceOutputItem> Output;
    public string OutputScreenName;
    public Sprite Icon;

    public override Sprite OutputIcon()
    {
        throw new System.NotImplementedException();
    }

    public override string OutputName()
    {
        throw new System.NotImplementedException();
    }

    public override (Sprite, int) ProduceOutput(BuildingCrafting ProducingBuilding)
    {
        throw new System.NotImplementedException();
    }

    protected override string CreateOutputDescription()
    {
        throw new System.NotImplementedException();
    }
}
