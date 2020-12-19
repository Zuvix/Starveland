using System;
using System.Linq;
using System.Collections.Generic;
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
        float Progress = (float)Math.Floor((float)OwnedParts / RequiredShipPartCount * 100);
        PrefabPallette.Instance.ShipProgressPerecentLabel.text = $"{Progress} %";

        int VisibleItemCount = Math.Min(Math.Min(ShipRecipe.Input.Count, GUIReference.Instance.ShipProgressMaterialIcons.Count), GUIReference.Instance.ShipProgressMaterialLabels.Count);
        for (int i = 0; i < VisibleItemCount; i++)
        {
            GUIReference.Instance.ShipProgressMaterialIcons[i].sprite = ShipRecipe.Input[i].itemInfo.icon;
            int CurrentItemCount = GlobalInventory.Instance.GetInventory().ContainsKey(ShipRecipe.Input[i].itemInfo.name) ? GlobalInventory.Instance.GetInventory()[ShipRecipe.Input[i].itemInfo.name].Amount : 0;
            GUIReference.Instance.ShipProgressMaterialLabels[i].text = $"{CurrentItemCount}/{ShipRecipe.Input[i].Amount}";

            GUIReference.Instance.ShipProgressMaterialIcons[i].gameObject.SetActive(true);
            GUIReference.Instance.ShipProgressMaterialLabels[i].gameObject.SetActive(true);
        }
        for (int i = VisibleItemCount; i < Math.Min(GUIReference.Instance.ShipProgressMaterialIcons.Count, GUIReference.Instance.ShipProgressMaterialLabels.Count) ; i++)
        {
            GUIReference.Instance.ShipProgressMaterialIcons[i].gameObject.SetActive(false);
            GUIReference.Instance.ShipProgressMaterialLabels[i].gameObject.SetActive(false);
        }
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