using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
public class InventoryMenu : MonoBehaviour
{
    public GameObject ItemUI;
    List<ItemShow> itemList;
    public int maxItems;
    [SerializeField]
    private Toggle foodToggle;
    private bool showFoodEnabled=true;
    [SerializeField]
    private Toggle itemToggle;
    private bool showItemsEnabled=true;
    private int pageId=0;
    public int maxItemCount=20;
    List<Resource> itemsToShow = new List<Resource>();

    private void Awake()
    {
        itemList = new List<ItemShow>();
        for(int i = 0; i < maxItemCount; i++)
        {
            GameObject itm = Instantiate(ItemUI, this.transform);
            itemList.Add(itm.GetComponent<ItemShow>());
            itm.SetActive(false);
        }
    }
    public void UpdateItemDisplay()
    {
        showItemsEnabled = itemToggle.isOn;
        AssignItems();
    }
    public void UpdateFoodDisplay()
    {
        showFoodEnabled = foodToggle.isOn;
        AssignItems();
    }
    public void AssignItems()
    {
        if (PanelControl.Instance.GetActivePanelID() == 0)
        {
            List<Resource> resources = GlobalInventory.Instance.GetInventory().Values.ToList();
            foreach (ItemShow panel in itemList)
            {
                panel.gameObject.SetActive(false);
            }
            itemsToShow = new List<Resource>();

            foreach (Resource r in resources)
            {
                if (r.itemInfo.type.Equals("Food") && !showFoodEnabled)
                {
                    continue;
                }
                if (r.itemInfo.type.Equals("Resource") && !showItemsEnabled)
                {
                    continue;
                }
                itemsToShow.Add(r);
            }
            int maxPageId = Mathf.CeilToInt((float)itemsToShow.Count / (float)maxItemCount);
            if (pageId >= maxPageId)
            {
                pageId = 0;
            }
            //Counter for UI elements
            int ui_i = 0;
            for (int i = pageId * maxItemCount; i < (pageId + 1) * maxItemCount; i++)
            {
                if (i < itemsToShow.Count)
                {
                    itemList[ui_i].ShowItem(itemsToShow[i]);
                    ui_i++;
                }
            }
            Debug.Log("MaxPageid" + maxPageId);
            Debug.Log("Pageid" + pageId);
        }
           
    }
    private void OnEnable()
    {
        pageId = 0;
    }
    public void UpPageId()
    {
        if ((pageId+1)*maxItemCount < itemsToShow.Count)
        {
            pageId++;
            AssignItems();
        }
    }
    public void DownPageId()
    {
        if (pageId>0)
        {
            pageId--;
            AssignItems();
        }
    }
}
