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

    public readonly int InventoryGridSizeRows = 4;
    public readonly int InventoryGridSizeColumns = 10;

    private Queue<UnitPlayer> PlayerUnits;

    private void Awake()
    {
        FeedingPanel.SetActive(false);
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

        /* List<GameObject> Icons = new List<GameObject>();
         Icons.Add(IconUL);
         Icons.Add(IconUR);
         Icons.Add(IconLL);
         Icons.Add(IconLR);*/

        /* Vector3 IconPosition = IconUL.transform.position;
         Debug.Log(IconPosition);

          IconPosition = IconUR.transform.position;
         Debug.Log(IconPosition);

          IconPosition = IconLL.transform.position;
         Debug.Log(IconPosition);

         IconPosition = IconLR.transform.position;
         Debug.Log(IconPosition);*/
        /*foreach (GameObject GameObject in Icons)
        {
            Debug.Log(GameObject.transform.localPosition);
        }
        foreach (GameObject GameObject in Icons)
        {
            Debug.Log(GameObject.GetComponent<RectTransform>().anchoredPosition);
        }
        foreach (GameObject GameObject in Icons)
        {
            Debug.Log(GameObject.GetComponent<RectTransform>().sizeDelta);
        }
        Vector3[] Corners = new Vector3[] { new Vector3(), new Vector3(), new Vector3(), new Vector3()};
        FeedingPanel.GetComponent<RectTransform>().GetWorldCorners(Corners);
        for (int i = 0; i < Corners.Length; i++)
        {
            Debug.Log(Corners[i]);
        }
        int j = 0;
        foreach (GameObject GameObject in Icons)
        {
            Corners = new Vector3[] { new Vector3(), new Vector3(), new Vector3(), new Vector3() };
            GameObject.GetComponent<RectTransform>().GetWorldCorners(Corners);
            Debug.Log("Icon number "+ j);
            for (int i = 0; i < Corners.Length; i++)
            {
                Debug.Log(Corners[i]);
            }
            j++;
        }*/
    }
}
