using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryMenu : MonoBehaviour
{
    public GameObject ItemUI;
    List<ItemShow> itemList;
    public int maxItems;
    private void Awake()
    {
        itemList = new List<ItemShow>();
        for(int i = 0; i < 16; i++)
        {
            GameObject itm = Instantiate(ItemUI, this.transform);
            itemList.Add(itm.GetComponent<ItemShow>());
            itm.SetActive(false);
        }
    }
    public void AssignItems()
    {
        Debug.LogWarning("Assigining items");
        List<Resource> resources = GlobalInventory.Instance.GetInventory();
        int i = 0;
        foreach(ItemShow panel in itemList)
        {
            panel.gameObject.SetActive(false);
        }
        foreach(Resource r in resources)
        {
            itemList[i].ShowItem(r);
        }
    }
}
