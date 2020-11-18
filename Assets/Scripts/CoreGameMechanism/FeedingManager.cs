using System.Collections.Generic;
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

    public readonly int InventoryGridSizeRows = 4;
    public readonly int InventoryGridSizeColumns = 10;

    private List<UnitPlayer> PlayerUnits;

    private void Awake()
    {
        FeedingPanel.SetActive(false);
        DraggedObject.SetActive(false);
        DraggedObject.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
        foreach (GameObject UnitPanel in UnitPanels)
        {
            UnitPanel.GetComponent<DroppableArea>().DroppedInArea.AddListener(DroppedInConsumationArea);
        }
        PlayerUnits = Unit.PlayerUnitPool.GetRange(0, Unit.PlayerUnitPool.Count);
    }
    public void InitiateDayEnd()
    {
        FeedingPanel.SetActive(true);
        FillGrid();

        PlayerUnits = Unit.PlayerUnitPool.GetRange(0, Unit.PlayerUnitPool.Count);
        FillUnitPanels();
    }
    public void DroppedInConsumationArea()
    {
        Debug.LogError("Dropped into this area");
    }
    public void InitiateDayStart()
    {
        FeedingPanel.SetActive(false);
        DayCycleManager.Instance.StartDay();
        ClearGrid();
    }
    private void FillUnitPanels()
    {
        GameObject Child;
        for (int i = 0; i < UnitPanels.Count && i < PlayerUnits.Count; i++)
        {
            UnitPanels[i].SetActive(true);
            for (int j = 0; j < UnitPanels[i].transform.childCount; j++)
            {
                Child = UnitPanels[i].transform.GetChild(j).gameObject;
                if (Child.name == "UnitName")
                {
                    Child.GetComponent<TextMeshProUGUI>().text = PlayerUnits[i].objectName;
                }
            }
        }
        for (int i = PlayerUnits.Count; i < UnitPanels.Count; i++)
        {
            UnitPanels[i].SetActive(false);
        }
    }
    private void FillGrid()
    {
        // https://answers.unity.com/questions/989697/grid-layout-group-scalable-content.html
        RectTransform parentRect = FoodInventoryPanel.GetComponent<RectTransform>();
        GridLayoutGroup gridLayout = FoodInventoryPanel.GetComponent<GridLayoutGroup>();

        AvailableItems = PrepareFoodList();

        gridLayout.cellSize = new Vector2(parentRect.rect.width / InventoryGridSizeColumns, parentRect.rect.height / InventoryGridSizeRows);
        int FoodListIndex;
        for (int i = 0; i < InventoryGridSizeRows; i++)
        {
            for (int j = 0; j < InventoryGridSizeColumns; j++)
            {
                FoodListIndex = i * InventoryGridSizeRows + j;
                if (FoodListIndex >= AvailableItems.Count)
                {
                    break;
                }

                GameObject InventoryItem = Instantiate(this.InventoryItem);
                InventoryItem.GetComponent<UnityEngine.UI.Image>().sprite = AvailableItems[FoodListIndex].itemInfo.icon;
                InventoryItem.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);

                InventoryItem.transform.SetParent(FoodInventoryPanel.transform, false);

                FoodInventoryItemPanels.Add(InventoryItem);
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
        List<Resource> Result = new List<Resource>();

        Result.Add(new Resource(ItemManager.Instance.GetItem("Apple"), 8));
        Result.Add(new Resource(ItemManager.Instance.GetItem("Carrot"), 4));
        Result.Add(new Resource(ItemManager.Instance.GetItem("Mushroom"), 3));
        Result.Add(new Resource(ItemManager.Instance.GetItem("Steak"), 6));

        return Result;
    }
}
