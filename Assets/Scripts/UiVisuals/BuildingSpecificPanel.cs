using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class BuildingSpecificPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text BuildingNameLabel;
    public Image BuildingIcon;
    public List<GameObject> OfferedItemPanels;
    public GameObject BuildingInfoPopupPanel;
    public GameObject Popup;
    public GameObject CraftProgressBar;
    private ProgressBar CraftProgress;
    public TMP_Text CratedItemNameLabel;

    private BuildingCrafting BoundBuilding = null;
    void Awake()
    {
        this.gameObject.SetActive(false);
    }
    void Start()
    {
        int i = 0;
        BuildingSpecificItemOfferPanel PanelComponent;
        foreach (GameObject ItemPanel in OfferedItemPanels)
        {
            PanelComponent = ItemPanel.GetComponent<BuildingSpecificItemOfferPanel>();
            PanelComponent.index = i;
            PanelComponent.SuperPanel = this;
            i++;
        }
        CraftProgress = CraftProgressBar.GetComponent<ProgressBar>();
        DaytimeCounter.Instance.OnDayOver.AddListener(Hide);
    }
    public void Display(BuildingCrafting Building)
    {
        this.BoundBuilding = Building;
        this.BuildingNameLabel.text = this.BoundBuilding.objectName;
        this.BuildingIcon.GetComponent<Image>().sprite = this.BoundBuilding.gameObject.GetComponent<SpriteRenderer>().sprite;
        MouseEvents.Instance.RegisterVisibleBuildingPanel(this);

        for (int i = 0; i < Building.AvailableRecipes.Count && i < OfferedItemPanels.Count; i++)
        {
            OfferedItemPanels[i].GetComponent<BuildingSpecificItemOfferPanel>().Initialize(Building.AvailableRecipes[i], Building);
            OfferedItemPanels[i].SetActive(true);
            OfferedItemPanels[i].GetComponent<BuildingSpecificItemOfferPanel>().QueueCountLabel.gameObject.SetActive(true);
        }
        for (int i = Building.AvailableRecipes.Count; i < OfferedItemPanels.Count; i++)
        {
            OfferedItemPanels[i].SetActive(false);
            OfferedItemPanels[i].GetComponent<BuildingSpecificItemOfferPanel>().QueueCountLabel.gameObject.SetActive(false);
        }
        if (Building.CurrentRecipeIndex != -1)
        {
            UpdateCurrentlyCraftedItemName(Building.AvailableRecipes[Building.CurrentRecipeIndex].OutputName());
        }

        this.BoundBuilding.OnQueueUpdate.AddListener(UpdateQuantityLabel);
        this.BoundBuilding.OnCraftStart.AddListener(UpdateCurrentlyCraftedItemName);
        this.BoundBuilding.OnCraftUpdate.AddListener(UpdateCraftProgress);
        this.BoundBuilding.OnCraftEnd.AddListener(HideCraftProgressBar);
        HideCraftProgressBar();
        /*this.BoundBuilding.*/BuildingCrafting.ToggleProgressBarVisibility(false);

        this.gameObject.SetActive(true);

    }
    public void Hide()
    {
        for (int i = 0; i < OfferedItemPanels.Count; i++)
        {
            OfferedItemPanels[i].GetComponent<BuildingSpecificItemOfferPanel>().Hide();
        }

        this.gameObject.SetActive(false);
        MouseEvents.Instance.UnregisterVisibleBuildingPanel();
        if (this.BoundBuilding != null)
        {
            this.BoundBuilding.OnQueueUpdate.RemoveListener(UpdateQuantityLabel);
            this.BoundBuilding.OnCraftStart.RemoveListener(UpdateCurrentlyCraftedItemName);
            this.BoundBuilding.OnCraftUpdate.RemoveListener(UpdateCraftProgress);
            this.BoundBuilding.OnCraftEnd.RemoveListener(HideCraftProgressBar);
            /*this.BoundBuilding*/BuildingCrafting.ToggleProgressBarVisibility(true);
            this.BoundBuilding = null;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEvents.Instance.DragEnabled = false;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MouseEvents.Instance.DragEnabled = true;
    }
    public void UpdateCurrentlyCraftedItemName(string Value)
    {
        this.CratedItemNameLabel.text = Value;
        this.CraftProgressBar.SetActive(true);
    }
    public void UpdateCraftProgress(float Value)
    {
        this.CraftProgress.CurrentProgress = Value;
        this.CraftProgressBar.SetActive(true);
    }
    private void HideCraftProgressBar()
    {
        this.CraftProgressBar.SetActive(false);
    }
    private void UpdateQuantityLabel(int Index, List<Resource> ResourcesSpent)
    {
        this.OfferedItemPanels[Index].GetComponent<BuildingSpecificItemOfferPanel>().UpdateQuantityLabel(ResourcesSpent);
    }
}
