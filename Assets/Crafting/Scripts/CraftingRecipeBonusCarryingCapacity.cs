using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newCraftingRecipeBonusCarryingCapacity", menuName = "CraftingRecipeBonusCarryingCapacity")]
public class CraftingRecipeBonusCarryingCapacity : CraftingRecipeGeneric
{
    public int Bonus;
    protected override void PerformRecipeAction()
    {
        foreach (UnitPlayer Unit in UnitManager.Instance.PlayerUnitPool)
        {
            Unit.CarryingCapacity += Bonus;
        }
    }
}
