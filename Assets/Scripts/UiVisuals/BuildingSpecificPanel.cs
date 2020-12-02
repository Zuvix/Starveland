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
        }
        for (int i = Building.AvailableRecipes.Count; i < OfferedItemPanels.Count; i++)
        {
            OfferedItemPanels[i].SetActive(false);
        }
        this.BoundBuilding.OnQueueUpdate.AddListener(UpdateQuantityLabel);

        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
        MouseEvents.Instance.UnregisterVisibleBuildingPanel();
        this.BoundBuilding.OnQueueUpdate.RemoveListener(UpdateQuantityLabel);
        this.BoundBuilding.ToggleProgressBarVisibility(true);
        this.BoundBuilding = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEvents.Instance.DragEnabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseEvents.Instance.DragEnabled = true;
    }
    private void UpdateQuantityLabel(int Index, List<Resource> ResourcesSpent)
    {
        this.OfferedItemPanels[Index].GetComponent<BuildingSpecificItemOfferPanel>().UpdateQuantityLabel(ResourcesSpent);
    }
}
