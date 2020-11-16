using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalInventory :Singleton<GlobalInventory>
{
    public UnityEvent OnInventoryUpdate = new UnityEvent();
    private List<Resource> playerInventory=new List<Resource>();
    public List<Resource> GetInventory()
    {
        return playerInventory;
    }
    public bool AddItem(Resource itemToAdd)
    {
        Debug.LogWarning("Adding item " + itemToAdd.itemInfo.name);
        foreach(Resource r in playerInventory)
        {
            if (r.itemInfo.name.Equals(itemToAdd.itemInfo.name))
            {
                r.Amount += itemToAdd.Amount;
                OnInventoryUpdate.Invoke();
                return true;
            }
            
        }
        //TODO change 24 to smth better
        if (playerInventory.Count <= 24)
        {
            playerInventory.Add(itemToAdd);
            OnInventoryUpdate.Invoke();
            return true;
        }
        Debug.LogWarning("Inventory was full, item failed to be added.");
        return false;
    }
}
