using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class BuildingSpecificItemOfferPanel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text QueueCountLabel;
    public int index;
    public BuildingSpecificPanel SuperPanel;

    private CraftingRecipe Recipe;
    private BuildingCrafting Building;

    private bool ResourcesAvailable = false;
    public void Initialize(CraftingRecipe Recipe, BuildingCrafting Building)
    {
        this.Recipe = Recipe;
        this.Building = Building;
        this.gameObject.GetComponent<Image>().sprite = this.Recipe.OutputIcon();
        UpdateQuantityLabel();

        CheckResourceAvailability();
        GlobalInventory.Instance.OnInventoryUpdate.AddListener(CheckResourceAvailability);
    }
    public void Hide()
    {
        GlobalInventory.Instance.OnInventoryUpdate.RemoveListener(CheckResourceAvailability);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool LeftClick = eventData.button == PointerEventData.InputButton.Left;
        bool RightClick = eventData.button == PointerEventData.InputButton.Right;

        int Amount = 1;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            Amount = 5;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (LeftClick)
            {
                Amount = GlobalInventory.Instance.CheckAvailabilityAmount(Recipe.Input);
            }
            else if (RightClick)
            {
                Amount = Building.ItemQuantities[index];
            }
        }
        if (LeftClick && ResourcesAvailable)
        {
            for (int i = 0; i < Amount; i++)
            {
                Building.EnqueueRecipe(index);
            }
        }
        else if (RightClick)
        {
            for (int i = 0; i < Amount; i++)
            {
                Building.CancelQueuedRecipe(index);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GlobalGameState.Instance.InGameInputAllowed)
        {
            SuperPanel.BuildingInfoPopupPanel.GetComponent<BuildingInfoPopupPanel>().Display(Recipe.OutputName(), Recipe.OutputDescription(), Recipe.Input);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SuperPanel.BuildingInfoPopupPanel.GetComponent<BuildingInfoPopupPanel>().Hide();
    }

    public void UpdateQuantityLabel()
    {
        this.QueueCountLabel.text = Building.ItemQuantities[index] > 0 ? Building.ItemQuantities[index].ToString() : "";
    }
    public void UpdateQuantityLabel(List<Resource> ResourcesSpent)
    {
        UpdateQuantityLabel();
        //CreatePopups(ResourcesSpent.Select(res => (res.itemInfo.icon, -res.Amount)).ToList());
    }
    public void CheckResourceAvailability()
    {
        ResourcesAvailable = true;
        foreach (Resource Resource in Recipe.Input)
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
    /*public void CreatePopup(Sprite icon, int value)
    {
        GameObject g = Instantiate(SuperPanel.Popup, this.gameObject.transform);
        if (g.GetComponentInChildren<ItemPopup>() == null)
        {
            Debug.LogError("BuildingSpecificItemOfferPanel::CreatePopup - ItemPopup component in children is null");
        }
        g.GetComponentInChildren<ItemPopup>()?.CreatePopup(icon, value);
    }
    public void CreatePopups(List<(Sprite, int)> Entries)
    {
        StartCoroutine(MultiPopupCoroutine(Entries));
    }
    private IEnumerator MultiPopupCoroutine(List<(Sprite, int)> Entries)
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            if (i > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }
            this.CreatePopup(Entries[i].Item1, Entries[i].Item2);
        }
    }*/
}
