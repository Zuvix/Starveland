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
        AddItem(new Resource(ItemManager.Instance.GetItem("Apple"), 5));
        AddItem(new Resource(ItemManager.Instance.GetItem("Carrot"), 4));
        AddItem(new Resource(ItemManager.Instance.GetItem("Mushroom"), 3));
        AddItem(new Resource(ItemManager.Instance.GetItem("Cooked Meat"), 3));
    }
    public bool AddItem(Resource itemToAdd)
    {
        if (CheckAvaliableItem(itemToAdd.itemInfo.name,1))
        {
            playerInventory[itemToAdd.itemInfo.name].AddDestructive(itemToAdd);
            OnInventoryUpdate.Invoke();
            return true;
        }
        playerInventory.Add(itemToAdd.itemInfo.name,itemToAdd);
        OnInventoryUpdate.Invoke();
        return true;
    }
    public bool CheckAvaliableItem(string itemName,int amountNeeded)
    {
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
            }
        }
        return Result;
    }
    public void RemoveUneatenFood()
    {
        Dictionary<string, Resource> newDict =new Dictionary<string, Resource>();
        foreach (KeyValuePair<string, Resource> entry in playerInventory)
        {
            if (!entry.Value.itemInfo.type.Equals("Food") || !entry.Value.itemInfo.storageType.Equals("Raw"))
            {
                newDict.Add(entry.Key, entry.Value);
                Debug.Log(entry.Value.itemInfo.type + "  " + entry.Value.itemInfo.storageType);
            }
        }
        playerInventory = newDict;
        OnInventoryUpdate.Invoke();
    }
}
