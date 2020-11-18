using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalInventory :Singleton<GlobalInventory>
{
    public UnityEvent OnInventoryUpdate = new UnityEvent();
    private Dictionary<string,Resource> playerInventory=new Dictionary<string, Resource>();

    public Dictionary<string,Resource> GetInventory()
    {
        return playerInventory;
    }
    public bool AddItem(Resource itemToAdd)
    {
        Debug.LogWarning("Adding item " + itemToAdd.itemInfo.name);
        if (CheckAvaliableItem(itemToAdd.itemInfo.name,1))
        {
            playerInventory[itemToAdd.itemInfo.name].AddDestructive(itemToAdd);
            OnInventoryUpdate.Invoke();
            return true;
        }
            
        //TODO change 24 to smth better
        if (playerInventory.Count <= 24)
        {
            playerInventory.Add(itemToAdd.itemInfo.name,itemToAdd);
            OnInventoryUpdate.Invoke();
            return true;
        }
        Debug.LogWarning("Inventory was full, item failed to be added.");
        return false;
    }
    public bool CheckAvaliableItem(string itemName,int amountNeeded)
    {
        if (playerInventory.ContainsKey(itemName))
        {
            if(playerInventory[itemName].Amount>= amountNeeded)
            {
                return true;
            }
        }
        return false;
    }
    public bool RemoveItem(string itemName,int amountToRemove)
    {
        if (CheckAvaliableItem(itemName, amountToRemove))
        {
            playerInventory[itemName].Subtract(amountToRemove);
            if (playerInventory[itemName].Amount == 0)
            {
                playerInventory.Remove(itemName);
            }
            OnInventoryUpdate.Invoke();
            return true;
        }

        return false;
    }
}
