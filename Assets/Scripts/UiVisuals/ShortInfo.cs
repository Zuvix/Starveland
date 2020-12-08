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
    public List<Image> VisitorImages;
    public List<TMP_Text> VisitorCounts;

    List<GameObject> contentPanels;

    CellObject visibleObject = null;
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

        if (visibleObject is Building)
        {
            ((Building)visibleObject).OnVisitorsChanged.RemoveListener(FillVisitorPanels);
        }
        visibleObject = go.GetComponent<CellObject>();
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
        else if (visibleObject is ResourceSource)
        {
            ResourceSource rs = (ResourceSource)visibleObject;
            resourcePanel.SetActive(true);
            resourcePanelItemsInfo.ShowInfo(rs.resource);
            resourceSourcTipTxt.text = visibleObject.tip;

        }
        else if(visibleObject is Building)
        {
            buildingPanel.SetActive(true);
            buildingTxt.text = visibleObject.tip;
            FillVisitorPanels((Building)visibleObject);
            ((Building)visibleObject).OnVisitorsChanged.AddListener(FillVisitorPanels);
        }
    }
    private void FillVisitorPanels(Building Building)
    {
        List<Unit> Visitors = Building.CurrentVisitors;
        Debug.LogWarning(Visitors.Count);
        Dictionary<Sprite, int> VisitorImageDict = new Dictionary<Sprite, int>();
        foreach (Unit Unit in Visitors)
        {
            Sprite UnitImage = Unit.GetComponent<SpriteRenderer>().sprite;
            if (!VisitorImageDict.ContainsKey(UnitImage))
            {
                VisitorImageDict.Add(UnitImage, 1);
            }
            else
            {
                VisitorImageDict[UnitImage]++;
            }
        }

        int VisitorTypeCount = 0;
        foreach (KeyValuePair<Sprite, int> Entry in VisitorImageDict)
        {
            VisitorImages[VisitorTypeCount].GetComponent<Image>().sprite = Entry.Key;
            VisitorCounts[VisitorTypeCount].text = Entry.Value.ToString();
            VisitorImages[VisitorTypeCount].gameObject.SetActive(true);
            VisitorTypeCount++;
        }
        for (int i = VisitorTypeCount; i < VisitorImages.Count; i++)
        {
            VisitorImages[i].gameObject.SetActive(false);
        }
    }
}
