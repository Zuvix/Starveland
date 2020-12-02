using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class BuildingSpecificItemOfferPanel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text QueueCountLabel;
    public int index;
    public BuildingSpecificPanel SuperPanel;

    private CraftingRecipe Recipe;
    private BuildingCrafting Building;
    public void Initialize(CraftingRecipe Recipe, BuildingCrafting Building)
    {
        this.Recipe = Recipe;
        this.Building = Building;
        this.gameObject.GetComponent<Image>().sprite = this.Recipe.Output.itemInfo.icon;
        UpdateQuantityLabel();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Building.EnqueueRecipe(index);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Building.DequeueRecipe(index);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Fill Popup Panel With info and display it
        SuperPanel.BuildingInfoPopupPanel.GetComponent<BuildingInfoPopupPanel>().Display(Recipe.Output.itemInfo.name, $"{Recipe.Output.itemInfo.NutritionValue} nv", Recipe.Input);
        // Change its position to fit our needs.
        // It will safely restore its position when hidden, so no need to worry it would break anything
        Vector3 NewPopupPosition = this.gameObject.transform.position;
        NewPopupPosition.x -= 130;
        SuperPanel.BuildingInfoPopupPanel.transform.position = NewPopupPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SuperPanel.BuildingInfoPopupPanel.GetComponent<BuildingInfoPopupPanel>().Hide();
    }

    public void UpdateQuantityLabel()
    {
        this.QueueCountLabel.text = Building.ItemQuantities[index] > 0 ? Building.ItemQuantities[index].ToString() : "";
    }
}
