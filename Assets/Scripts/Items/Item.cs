using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName= "Item")] 
public class Item : ScriptableObject
{
    public ItemType ItemType;
    public Sprite icon;
    public int NutritionValue;
    public FoodStorageType storageType;
    public FoodType FoodType;
    public ItemRarity Rarity;
    public string description;
    public bool isHeavy;
}
