using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class FeedingManager : Singleton<FeedingManager>
{
    public GameObject FeedingPanel;
    public GameObject FoodInventoryPanel;
    public GameObject UnitListPanel;
    public List<GameObject> UnitPanels;

    public GameObject InventoryItem;
    public GameObject DraggedObject;

    public readonly int InventoryGridSizeRows = 4;
    public readonly int InventoryGridSizeColumns = 10;

    private Queue<UnitPlayer> PlayerUnits;

    private void Awake()
    {
        FeedingPanel.SetActive(false);
        DraggedObject.SetActive(false);
        DraggedObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
    private void FillGrid()
    {
        Debug.LogError("Filling grid");
        // https://answers.unity.com/questions/989697/grid-layout-group-scalable-content.html
        RectTransform parentRect = FoodInventoryPanel.GetComponent<RectTransform>();
        GridLayoutGroup gridLayout = FoodInventoryPanel.GetComponent<GridLayoutGroup>();

        if (parentRect == null) { Debug.LogError("Rect is null"); }
        if (gridLayout == null) { Debug.LogError("Grid Layout is null"); }

        gridLayout.cellSize = new Vector2(parentRect.rect.width / InventoryGridSizeColumns, parentRect.rect.height / InventoryGridSizeRows);
        for (int i = 0; i < InventoryGridSizeRows; i++)
        {
            for (int j = 0; j < InventoryGridSizeColumns; j++)
            {
                GameObject InventoryItem = Instantiate(this.InventoryItem);
                Item Apple = ItemManager.Instance.GetItem("Apple");
                InventoryItem.GetComponent<Image>().sprite = Apple.icon;
                InventoryItem.GetComponent<Image>().color = new Color(1, 1, 1, 1);


                InventoryItem.transform.SetParent(FoodInventoryPanel.transform, false);
                //InventoryItem.GetComponent<Draggable>().OriginalPosition = InventoryItem.transform.position;
            }
        }
    }
    public void InitiateDayEnd()
    {
        FeedingPanel.SetActive(true);
        Debug.Log("Showing Day End");
        FillGrid();

        foreach (GameObject UnitPanel in UnitPanels)
        {
            UnitPanel.GetComponent<Droppable>().DroppedInArea.AddListener(DroppedInArea);
        }
    }
    public void DroppedInArea()
    {
        Debug.LogError("Dropped into this area");
    }
    public void InitiateDayStart(string _)
    {
        FeedingPanel.SetActive(false);
        DayCycleManager.Instance.StartDay();
    }
}
