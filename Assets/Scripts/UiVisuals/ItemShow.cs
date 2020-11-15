using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemShow : MonoBehaviour
{
    public TMP_Text nameTxt;
    public TMP_Text amountTxt;
    public Image icon;
    public void ShowItem(Resource resource)
    {
        nameTxt.text = resource.itemInfo.name;
        amountTxt.text = resource.Amount.ToString();
        icon.sprite = resource.itemInfo.icon;
        this.gameObject.SetActive(true);
    }
}
