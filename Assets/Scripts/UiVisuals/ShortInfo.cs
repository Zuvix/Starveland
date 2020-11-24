using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class ShortInfo : MonoBehaviour
{
    public GameObject topContent;
    public TMP_Text nameTxt;
    public Image img;
    public TMP_Text tipTxt;

    public GameObject unitContent;
    public TMP_Text unitHP;
    public TMP_Text unitAction;
    public Image itemInHandImg;
    public TMP_Text itemAmountTxt;

    public GameObject resourcePanel;
    private ResourceShortInfo resourcePanelItemsInfo;
    public TMP_Text resourceSourcTipTxt;

    public GameObject buildingPanel;
    public TMP_Text buildingTxt;

    List<GameObject> contentPanels;
    private void Awake()
    {
        contentPanels = new List<GameObject>()
        {
            topContent,
            unitContent,
            resourcePanel,
            buildingPanel
        };
        HideTopContent();
        resourcePanelItemsInfo = resourcePanel.GetComponent<ResourceShortInfo>();
    }
    private void Start()
    {
        MouseEvents.Instance.viewObjectChanged.AddListener(UpdateTextInfo);
    }
    public void HideTopContent()
    {
        foreach(GameObject contentPanel in contentPanels)
        {
            contentPanel.SetActive(false);
        }
    }
    public void UpdateTextInfo(GameObject go, bool isSelected)
    {
        HideTopContent();
        if (go==null)
        {
            HideTopContent();
            return;
        }
        CellObject visibleObject = go.GetComponent<CellObject>();
        if (visibleObject!=null)
        {
            topContent.SetActive(true);
            img.sprite = visibleObject.sr.sprite;
            nameTxt.text = visibleObject.objectName;
        }
        if (visibleObject is Unit)
        {
            unitContent.SetActive(true);
            Unit unit = (Unit)visibleObject;
            unitHP.text = $"{unit.Health}/{unit.MaxHealth}";
            unitAction.text = unit.CurrentAction;
            tipTxt.text = unit.tip;
            Resource unitResource = unit.CarriedResource;
            if (unitResource.itemInfo != null)
            {
                if (unitResource.Amount >= 1)
                {
                    itemInHandImg.gameObject.SetActive(true);
                    itemAmountTxt.gameObject.SetActive(true);
                    itemInHandImg.sprite = unitResource.itemInfo.icon;
                    itemAmountTxt.text = unitResource.Amount.ToString();
                }
                
            }
            else
            {
                itemInHandImg.gameObject.SetActive(false);
                itemAmountTxt.gameObject.SetActive(false);
            }
        }
        if (visibleObject is ResourceSource)
        {
            ResourceSource rs = (ResourceSource)visibleObject;
            resourcePanel.SetActive(true);
            resourcePanelItemsInfo.ShowInfo(rs.Resources);
            resourceSourcTipTxt.text = visibleObject.tip;

        }
        if(visibleObject is Building)
        {
            buildingPanel.SetActive(true);
            buildingTxt.text = visibleObject.tip;
        }
    }
}
