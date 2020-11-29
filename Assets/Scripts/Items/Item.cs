using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName= "Item")] 
public class Item : ScriptableObject
{
    [StringInList("Resource","Food")]
    public string type;
    public Sprite icon;
    public int NutritionValue;
    [StringInList("Raw", "Cooked")]
    public string storageType;
}
