using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName= "Item")] 
public class Item : ScriptableObject
{
    public ItemType type;
    public Sprite icon;
    public int NutritionValue;
    public FoodStorageType storageType;
    [StringInList("None", "Meat", "Foragable")]
    public string foodType;
    [StringInList("Basic", "Processed", "Ship part","Legendary item")]
    public string rarity;
    public string description;
    public bool isHeavy;
    public bool IsShipPart;
}
