using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class BuildingSpecificPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text BuildingNameLabel;
    public Image BuildingIcon;

    private BuildingCrafting BoundBuilding = null;
    void Awake()
    {
        this.gameObject.SetActive(false);
    }
    public void Display(BuildingCrafting Building)
    {
        this.BoundBuilding = Building;
        this.BuildingNameLabel.text = this.BoundBuilding.objectName;
        this.BuildingIcon.GetComponent<Image>().sprite = this.BoundBuilding.gameObject.GetComponent<SpriteRenderer>().sprite;
        MouseEvents.Instance.RegisterVisibleBuildingPanel(this);

        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.BoundBuilding = null;
        this.gameObject.SetActive(false);
        MouseEvents.Instance.UnregisterVisibleBuildingPanel();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEvents.Instance.DragEnabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseEvents.Instance.DragEnabled = true;
    }
}
