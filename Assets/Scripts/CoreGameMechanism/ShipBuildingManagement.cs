using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuildingManagement : Singleton<ShipBuildingManagement>
{
    public CraftingRecipeBuildShip ShipRecipe;
    private int RequiredShipPartCount;
    private void Start()
    {
        RequiredShipPartCount = ShipRecipe.Input.Select(x => x.Amount).Sum();
        RefreshShipState();
        GlobalInventory.Instance.OnShipPartUpdate.AddListener(RefreshShipState);
    }
    private void RefreshShipState()
    {
        int OwnedParts = GlobalInventory.Instance.OwnedShipParts().Select(x => Math.Min(x.Amount, RequiredAmount(x.itemInfo))).Sum();
        float Progress = (float)OwnedParts / RequiredShipPartCount;
        Debug.LogError($"Owned {OwnedParts} ship parts out of {RequiredShipPartCount}, which makes it {(int)(Progress*100)}%.");
    }
    private int RequiredAmount(Item ItemInfo)
    {
        int Result = 0;
        List<Resource> RequiredResource = ShipRecipe.Input.Where(x => x.itemInfo == ItemInfo).ToList();
        if (RequiredResource.Count > 0)
        {
            Result = RequiredResource[0].Amount;
        }
        return Result;
    }
}
