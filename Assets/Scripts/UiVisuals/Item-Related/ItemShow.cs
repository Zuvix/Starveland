using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text nameTxt;
    public TMP_Text amountTxt;
    public Image icon;
    Resource rs;
    public void ShowItem(Resource resource)
    {
        rs = resource;
        nameTxt.text = resource.itemInfo.name;
        amountTxt.text = resource.Amount.ToString();
        icon.sprite = resource.itemInfo.icon;
        this.gameObject.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rs != null)
        {
                ItemInfo.Instance.SetFoodInfo(rs.itemInfo);
        }

    }
    private void OnMouseExit()
    {
        ItemInfo.Instance.Deactivate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfo.Instance.Deactivate();
    }
}
