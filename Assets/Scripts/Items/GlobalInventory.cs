using System;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
public class GlobalInventory : Singleton<GlobalInventory>
{
    public UnityEvent OnInventoryUpdate = new UnityEvent();
    public UnityEvent OnShipPartUpdate = new UnityEvent();
    private void ActionOnInventoryUpdate(Item ChangedItemInfo)
    {
        OnInventoryUpdate.Invoke();
        if (ChangedItemInfo.Rarity == ItemRarity.Ship_Part)
        {
            OnShipPartUpdate.Invoke();
        }
    }
    private Dictionary<string,Resource> playerInventory=new Dictionary<string, Resource>();
    public Dictionary<string,Resource> GetInventory()
    {
        return playerInventory;
    }
    private void Start()
    {
        AddItem(new Resource(ItemManager.Instance.GetItem("Can of perfection"), 5));

        foreach (string ResType in ItemManager.Instance.GetItemTypeNames())
        {
            AddItem(new Resource(ItemManager.Instance.GetItem(ResType), 200));
        }
    }
    public bool AddItem(Resource itemToAdd)
    {
        Item AddedItemInfo = itemToAdd.itemInfo;
        if (CheckAvaliableItem(itemToAdd.itemInfo.name,1))
        {
            playerInventory[itemToAdd.itemInfo.name].AddDestructive(itemToAdd);
            ActionOnInventoryUpdate(AddedItemInfo);
            return true;
        }
        playerInventory.Add(itemToAdd.itemInfo.name,itemToAdd);
        ActionOnInventoryUpdate(AddedItemInfo);
        return true;
    }
    public bool AddItems(List<Resource> itemsToAdd)
    {
        bool Result = true;
        foreach (Resource itemToAdd in itemsToAdd)
        {
            Result &= AddItem(itemToAdd);
        }
        return Result;
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
    public int CheckAvailabilityAmount(List<Resource> Ingredients)
    {
        int Result = CheckAvailabilityAmount(Ingredients[0]);
        for (int i = 1; i < Ingredients.Count; i++)
        {
            Result = Math.Min(Result, CheckAvailabilityAmount(Ingredients[i]));
        }
        return Result;
    }
    private int CheckAvailabilityAmount(Resource Ingredient)
    {
        int Result = 0;
        if (playerInventory.ContainsKey(Ingredient.itemInfo.name))
        {
            Result = (int) Math.Floor(playerInventory[Ingredient.itemInfo.name].Amount / (float) Ingredient.Amount);
        }
        return Result;
    }
    public bool RemoveItem(string itemName,int amountToRemove)
    {
        if (CheckAvaliableItem(itemName, amountToRemove))
        {
            Item TakenItemInfo = playerInventory[itemName].itemInfo;
            playerInventory[itemName].Subtract(amountToRemove);
            if (playerInventory[itemName].Amount == 0)
            {
                playerInventory.Remove(itemName);
            }
            ActionOnInventoryUpdate(TakenItemInfo);
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
                Item TakenItemInfo = item.itemInfo;
                playerInventory[item.itemInfo.name].Subtract(item.Amount);
                if (playerInventory[itemKey].Amount == 0)
                {
                    playerInventory.Remove(itemKey);
                }
                ActionOnInventoryUpdate(TakenItemInfo);
                return true;
            }
        }

        return false;
    }
    public bool AttemptRemoveItems(List<Resource> Items)
    {
        bool Success = true;
        List<Resource> BackedUpResources = new List<Resource>();

        foreach (Resource Resource in Items)
        {
            Success &= RemoveItem(Resource);
            if (!Success)
            {
                break;
            }
            BackedUpResources.Add(Resource);
        }

        if (!Success)
        {
            foreach (Resource Resource in BackedUpResources)
            {
                AddItem(Resource);
            }
        }

        return Success;
    }
    public List<Resource> AvailableFood()
    {
        List<Resource> Result = new List<Resource>();
        foreach (KeyValuePair<string, Resource> entry in playerInventory)
        {
            if (entry.Value.itemInfo.ItemType == ItemType.Food)
            {
                Result.Add(entry.Value.Duplicate());
            }
        }
        return Result;
    }
    public List<Resource> OwnedShipParts()
    {
        return playerInventory.Values.Where(x => x.itemInfo.Rarity == ItemRarity.Ship_Part).ToList();
    }
    public void RemoveUneatenFood()
    {
        Dictionary<string, Resource> newDict =new Dictionary<string, Resource>();
        foreach (KeyValuePair<string, Resource> entry in playerInventory)
        {
            if (entry.Value.itemInfo.ItemType != ItemType.Food || entry.Value.itemInfo.storageType != FoodStorageType.Raw)
            {
                newDict.Add(entry.Key, entry.Value);
            }
        }
        playerInventory = newDict;
        OnInventoryUpdate.Invoke();
    }
}