using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingOfferPanel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject BuildingPanel;

    public GameObject OfferedBuilding;
    private Building BuildingComponent;
    private bool ResourcesAvailable;

    private BuildingInfoPopupPanel PopupPanel;
    public void Awake()
    {
        this.BuildingComponent = OfferedBuilding.GetComponent<Building>();
        CheckResourceAvailability();
        GlobalInventory.Instance.OnInventoryUpdate.AddListener(CheckResourceAvailability);
    }
    public void Start()
    {
        PopupPanel = BuildingPanel.GetComponent<BuildingPanel>().PopupPanel.GetComponent<BuildingInfoPopupPanel>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Building {this.BuildingComponent.name} clicked");
        if (ResourcesAvailable)
        {
            BuildingConstructionManager.Instance.SelectBuilding(OfferedBuilding);
        }
        else
        {
            //TODO
        }
    }
    public void CheckResourceAvailability()
    {
        ResourcesAvailable = true;
        foreach (Resource Resource in BuildingComponent.ConstructionCost)
        {
            ResourcesAvailable &= GlobalInventory.Instance.CheckAvaliableItem(Resource.itemInfo.name, Resource.Amount);
            if (!ResourcesAvailable)
            {
                break;
            }
        }

        MakeAvailable();
    }
    private void MakeAvailable()
    {
        if (ResourcesAvailable)
        {
            this.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            this.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.25f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // BuildingConstructionManager.Instance.PopupPanel.GetComponent<BuildingInfoPopupPanel>().Display(BuildingComponent);
        PopupPanel.Display(BuildingComponent.objectName, BuildingComponent.tip, BuildingComponent.ConstructionCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // BuildingConstructionManager.Instance.PopupPanel.SetActive(false);
        PopupPanel.Hide();
    }
}
