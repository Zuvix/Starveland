using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingOfferPanel : MonoBehaviour, IPointerClickHandler
{
    public GameObject OfferedBuilding;
    private Building BuildingComponent;
    public void Awake()
    {
        this.BuildingComponent = OfferedBuilding.GetComponent<Building>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Building {this.BuildingComponent.name} clicked");
        BuildingConstructionManager.Instance.SelectBuilding(OfferedBuilding);
    }
}
