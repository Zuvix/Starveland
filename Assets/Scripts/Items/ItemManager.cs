﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : Singleton<ItemManager>
{
    private Dictionary<string,Item> items;
    public Dictionary<Item, RSObjects> resourceToResourceSource; 
    private void Awake()
    {
        items = new Dictionary<string, Item>();
        LoadItemsFromDirectory("Items/Materials/");
        LoadItemsFromDirectory("Items/Food/");
        resourceToResourceSource = new Dictionary<Item, RSObjects>()
        {
            { GetItem("Meat"), RSObjects.DeadAnimalMeat },
            { GetItem("Fur"), RSObjects.DeadAnimalFur },
            { GetItem("Diamond"), RSObjects.Diamond },
            { GetItem("Magic Scroll"), RSObjects.Scroll }
        };
    }
    void LoadItemsFromDirectory(string path)
    {
        Object[] assets = Resources.LoadAll(path);
        if (assets == null || assets.Length == 0)
        {
            Debug.Log("Files not found");
        }

        foreach (Object ass in assets)
        {
            if(ass is Item)
            {
                Item item = (Item)ass;
                items.Add(item.name,item);
            }
        }
    }
    public Item GetItem(string item)
    {
        if (items.ContainsKey(item))
        {
            return items[item];
        }
        else
        {
            Debug.LogWarning("item with key " + item + " doesnt exist");
            return null;
        }
    }
    public List<string> GetItemTypeNames()
    {
        return new List<string>(items.Keys);
    }
}
