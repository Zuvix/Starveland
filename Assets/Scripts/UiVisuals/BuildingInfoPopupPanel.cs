using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuildingInfoPopupPanel : MonoBehaviour
{
    public TMP_Text BuildingNameLabel;
    public TMP_Text BuildingDescriptionLabel;
    public List<GameObject> ResourcePanels;
    private Building CurrentBuilding;

    private static readonly Color DefaultColor = new Color(1, 1, 1, 0.3921f);
    private static readonly Color UnavailableColor = new Color(0.9811f, 0.3831f, 0.3831f, 0.533f);
    void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public void Display(Building Building)
    {

        BuildingNameLabel.text = Building.objectName;
        BuildingDescriptionLabel.text = Building.tip;
        CurrentBuilding = Building;
        DisplayResourcePanels();

        GlobalInventory.Instance.OnInventoryUpdate.AddListener(DisplayResourcePanels);

        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        CurrentBuilding = null;
        GlobalInventory.Instance.OnInventoryUpdate.RemoveListener(DisplayResourcePanels);
        this.gameObject.SetActive(false);
    }
    private void DisplayResourcePanels()
    {
        for (int i = 0; i < CurrentBuilding.ConstructionCost.Count && i < ResourcePanels.Count; i++)
        {
            ResourcePanels[i].GetComponent<BuildingInfoResourcePanel>().Fill(CurrentBuilding.ConstructionCost[i]);
            CheckResourceAvailability(CurrentBuilding.ConstructionCost[i], ResourcePanels[i]);
            ResourcePanels[i].SetActive(true);
        }
        for (int i = CurrentBuilding.ConstructionCost.Count; i < ResourcePanels.Count; i++)
        {
            ResourcePanels[i].SetActive(false);
        }
    }
    private void CheckResourceAvailability(Resource Resource, GameObject ResourcePanel)
    {
        if (GlobalInventory.Instance.CheckAvaliableItem(Resource.itemInfo.name, Resource.Amount))
        {
            ResourcePanel.GetComponent<Image>().color = DefaultColor;
        }
        else
        {
            ResourcePanel.GetComponent<Image>().color = UnavailableColor;
        }
    }
}