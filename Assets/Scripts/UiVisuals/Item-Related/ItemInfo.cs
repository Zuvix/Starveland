using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInfo : Singleton<ItemInfo>
{
    public TMP_Text foodName;
    public TMP_Text itemType;
    public TMP_Text nutritionValue;
    public TMP_Text desc;
    public TMP_Text perservable;
    public Image icon;
    public GameObject FoodPanel;

    public TMP_Text labelNut;
    public TMP_Text labelPerv;

    public void SetFoodInfo(Item item)
    {
        if (item.type.Equals(ItemType.Food))
        {
            labelNut.text = "Nutrition Value:";
            labelPerv.text = "Perservation:";
            FoodPanel.SetActive(true);
            foodName.text = item.name;
            itemType.text = "Food";
            nutritionValue.text = item.NutritionValue + "nv";
            desc.text = item.description;
            icon.sprite = item.icon;
            perservable.text = item.storageType.ToString();
        }
        else
        {
            labelNut.text = "State:";
            labelPerv.text = "Is heavy:";
            FoodPanel.SetActive(true);
            foodName.text = item.name;
            itemType.text = "Material";
            nutritionValue.text = item.rarity;
            desc.text = item.description;
            icon.sprite = item.icon;
            perservable.text = item.isHeavy.ToString();
        }
    }
    public void Deactivate()
    {
        FoodPanel.SetActive(false);
    }
    private void Awake()
    {
        FoodPanel.SetActive(false);
    }
}
