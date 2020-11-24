using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

class FeedingManager : Singleton<FeedingManager>
{
    public GameObject FeedingPanel;
    public GameObject FoodInventoryPanel;
    private List<GameObject> FoodInventoryItemPanels = new List<GameObject>();
    public GameObject UnitListPanel;
    public List<GameObject> UnitPanels;

    private List<Resource> AvailableItems = null;

    public GameObject InventoryItem;
    public GameObject DraggedObject;
    public GameObject SelectedFoodIcon; 

    public readonly int InventoryGridSizeRows = 4;
    public readonly int InventoryGridSizeColumns = 10;

    private List<UnitHungry> PlayerUnits;

    private void Awake()
    {
        FeedingPanel.SetActive(false);
        DraggedObject.SetActive(false);
        DraggedObject.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
        foreach (GameObject UnitPanel in UnitPanels)
        {
            UnitPanel.GetComponent<DroppableArea>().DroppedInArea.AddListener(DroppedInConsumationArea);
        }
    }
    public void InitiateDayEnd()
    {
        FeedingPanel.SetActive(true);
        FillGrid();

        PlayerUnits = Unit.PlayerUnitPool.Select(unit => new UnitHungry(unit)).ToList();
        FillUnitPanels();
    }
    public void DroppedInConsumationArea(UnitPanel UnitPanel)
    {
        Resource ConsumedResource = SelectedFoodIcon.GetComponent<DraggableIcon>().Resource;
        UnitHungry ConsumingUnit = UnitPanel.Unit;
        if (GlobalInventory.Instance.RemoveItem(ConsumedResource.itemInfo.name, ConsumedResource.Amount))
        {
            ConsumingUnit.Eat(ConsumedResource.itemInfo.NutritionValue);
            if (ConsumingUnit.IsFed())
            {
                PlayerUnits.Remove(ConsumingUnit);
                FillUnitPanels();
            }

            FoodInventoryItemPanels.Remove(SelectedFoodIcon);
            Destroy(SelectedFoodIcon);
            SelectedFoodIcon = null;

            RefreshGrid();
        }
        else
        {
            Debug.LogError($"Tried to consume item that's not available: {ConsumedResource.Amount} of {ConsumedResource.itemInfo.name}");
        }
    }
    public void InitiateDayStart()
    {
        foreach (UnitHungry Unit in PlayerUnits)
        {
            Unit.Unit.Die();
        }

        if (!GameOver.Instance.GameIsOver)
        {
            FeedingPanel.SetActive(false);
            DayCycleManager.Instance.StartDay();
            ClearGrid();
        }
    }
    private void FillUnitPanels()
    {
        GameObject Child;
        for (int i = 0; i < UnitPanels.Count && i < PlayerUnits.Count; i++)
        {
            UnitPanels[i].SetActive(true);
            UnitPanels[i].GetComponent<UnitPanel>().Unit = PlayerUnits[i];
            UnitPanels[i].GetComponent<UnitPanel>().Unit.OnEatUpgrade.RemoveAllListeners();
            UnitPanels[i].GetComponent<UnitPanel>().Unit.OnEatUpgrade.AddListener(UnitPanels[i].GetComponent<UnitPanel>().UpdateSlider);
            PlayerUnits[i].Eat(0);
            for (int j = 0; j < UnitPanels[i].transform.childCount; j++)
            {
                Child = UnitPanels[i].transform.GetChild(j).gameObject;
                if (Child.name == "UnitName")
                {
                    Child.GetComponent<TextMeshProUGUI>().text = PlayerUnits[i].Unit.objectName;
                }
            }
        }
        for (int i = PlayerUnits.Count; i < UnitPanels.Count; i++)
        {
            UnitPanels[i].SetActive(false);
            UnitPanels[i].GetComponent<UnitPanel>().Unit = null;
            UnitPanels[i].GetComponent<UnitPanel>().UpdateSlider(0);
        }
    }
    private void RefreshGrid()
    {
        AddToGrid();
    }
    private void AddToGrid()
    {
        if (AvailableItems.Count > 0)
        {
            GameObject InventoryItem = Instantiate(this.InventoryItem);
            InventoryItem.GetComponent<UnityEngine.UI.Image>().sprite = AvailableItems[0].itemInfo.icon;
            InventoryItem.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
            InventoryItem.GetComponent<DraggableIcon>().Resource = AvailableItems[0];

            InventoryItem.transform.SetParent(FoodInventoryPanel.transform, false);

            FoodInventoryItemPanels.Add(InventoryItem);
            AvailableItems.RemoveAt(0);

            InventoryItem.SetActive(true);
        }
    }
    private void FillGrid()
    {
        // https://answers.unity.com/questions/989697/grid-layout-group-scalable-content.html
        RectTransform parentRect = FoodInventoryPanel.GetComponent<RectTransform>();
        GridLayoutGroup gridLayout = FoodInventoryPanel.GetComponent<GridLayoutGroup>();

        AvailableItems = PrepareFoodList();

        gridLayout.cellSize = new Vector2(parentRect.rect.width / InventoryGridSizeColumns, parentRect.rect.height / InventoryGridSizeRows);
        for (int i = 0; i < InventoryGridSizeRows; i++)
        {
            for (int j = 0; j < InventoryGridSizeColumns; j++)
            {
                if (AvailableItems.Count <= 0)
                {
                    break;
                }

                AddToGrid();
            }
        }
    }
    private void ClearGrid()
    {
        foreach (GameObject FoodInventoryItemPanel in FoodInventoryItemPanels)
        {
            Destroy(FoodInventoryItemPanel);
        }
        FoodInventoryItemPanels.Clear();
        AvailableItems.Clear();
    }
    private List<Resource> PrepareFoodList()
    {
        List<Resource> Result = new List<Resource>();
        List<Resource> AvailableResources = RetrieveAvailableFood();
        //Cannot deplete resources
        foreach (Resource Resource in AvailableResources)
        {
            while (!Resource.IsDepleted())
            {
                Result.Add(Resource.Subtract(1));
            }
        }
        ListShuffling.Shuffle<Resource>(Result);
        return Result;
    }
    private List<Resource> RetrieveAvailableFood()
    {
        List<Resource> Result = GlobalInventory.Instance.AvailableFood();

        return Result;
    }
}
