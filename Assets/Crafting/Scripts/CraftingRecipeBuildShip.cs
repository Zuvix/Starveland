using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newCraftingRecipeShipBuild", menuName = "CraftingRecipeShipBuild")]
public class CraftingRecipeBuildShip : CraftingRecipeGeneric
{
    protected override void PerformRecipeAction()
    {
        GameOver.Instance.InitiatePositiveGameOver();
    }
}
