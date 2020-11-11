using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName= "Item")] 
public class Item : ScriptableObject
{
    public string itemName;
    [StringInList("resource","food")]
    public string type;
    public Sprite icon;
    
}
