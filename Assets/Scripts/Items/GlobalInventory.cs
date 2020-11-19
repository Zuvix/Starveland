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
    private void Start()
    {
        Debug.Log(playerInventory.Count);

        AddItem(new Resource(ItemManager.Instance.GetItem("Apple"), 8));
        AddItem(new Resource(ItemManager.Instance.GetItem("Carrot"), 4));
        AddItem(new Resource(ItemManager.Instance.GetItem("Mushroom"), 3));
        AddItem(new Resource(ItemManager.Instance.GetItem("Steak"), 6));
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
        Debug.Log("Checking item availability");
        if (playerInventory.ContainsKey(itemName))
        {
            if(playerInventory[itemName].Amount >= amountNeeded)
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
    public bool RemoveItem(Resource item)
    {
        if (item != null)
        {
            string itemKey = item.itemInfo.name;
            if (CheckAvaliableItem(item.itemInfo.name, item.Amount))
            {
                playerInventory[item.itemInfo.name].Subtract(item.Amount);
                if (playerInventory[itemKey].Amount == 0)
                {
                    playerInventory.Remove(itemKey);
                }
                OnInventoryUpdate.Invoke();
                return true;
            }
        }

        return false;
    }
    public List<Resource> AvailableFood()
    {
        List<Resource> Result = new List<Resource>();
        foreach (KeyValuePair<string, Resource> entry in playerInventory)
        {
            if (entry.Value.itemInfo.type == "Food")
            {
                Result.Add(entry.Value.Duplicate());
                Debug.Log($"Added available food {entry.Key}");
            }
        }
        return Result;
    }
}
