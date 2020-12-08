using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CraftingRecipeGeneric : CraftingRecipe
{
    public Sprite Icon;
    public string ScreenName;
    public string ScreenDescription;

    public override (Sprite, int) ProduceOutput(BuildingCrafting ProducingBuilding)
    {
        PerformRecipeAction();
        return (Icon, 1);
    }
    protected abstract void PerformRecipeAction();
    public override Sprite OutputIcon()
    {
        return Icon;
    }

    public override string OutputName()
    {
        return ScreenName;
    }
    protected override string CreateOutputDescription()
    {
        return ScreenDescription;
    }
}