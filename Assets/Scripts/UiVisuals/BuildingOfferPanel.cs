using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingOfferPanel : MonoBehaviour, IPointerClickHandler
{
    public GameObject OfferedBuilding;
    private Building BuildingComponent;
    private bool ResourcesAvailable;
    public void Awake()
    {
        this.BuildingComponent = OfferedBuilding.GetComponent<Building>();
        CheckResourceAvailability();
        GlobalInventory.Instance.OnInventoryUpdate.AddListener(CheckResourceAvailability);
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
}
