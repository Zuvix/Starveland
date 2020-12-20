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
    public TMP_Text surTxt;

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

    public GameObject animalPanel;
    public TMP_Text animalHP;
    public TMP_Text animalDiff;
    public TMP_Text animalTip;
    public Image[] animalResources;

    public GameObject fishPanel;
    public FishItemUi[] fishItems; 

    List<GameObject> contentPanels;

    CellObject visibleObject = null;
    private void Awake()
    {
        contentPanels = new List<GameObject>()
        {
            topContent,
            unitContent,
            resourcePanel,
            buildingPanel,
            animalPanel,
            fishPanel
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
            if (visibleObject.CurrentCell != null)
            {
                surTxt.text = "X:" + visibleObject.CurrentCell.x +"\n"+ "Y:" + visibleObject.CurrentCell.y;
            }
        }
        if (visibleObject is UnitPlayer)
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
            //buildingTxt.text = visibleObject.tip;
            FillVisitorPanels((Building)visibleObject);
            ((Building)visibleObject).OnVisitorsChanged.AddListener(FillVisitorPanels);
        }
        else if(visibleObject is UnitAnimal)
        {
            animalPanel.SetActive(true);
            UnitAnimal animal = (UnitAnimal)visibleObject;
            animalDiff.text = animal.difficulty;
            animalHP.text = animal.Health+"/"+animal.MaxHealth;
            animalTip.text = animal.tip;
            int i;
            for (i = 0; i < animalResources.Length; i++)
            {
                animalResources[i].gameObject.SetActive(false);
            }
            i = 0;
            foreach(ResourcePack rp in animal.inventory)
            {
                if (i < animalResources.Length)
                {
                    animalResources[i].gameObject.SetActive(true);
                    animalResources[i].sprite = rp.item.icon;
                    i++;
                }
            }
        }
        else if (visibleObject is ResourceSourceFishing)
        {
            fishPanel.SetActive(true);
            ResourceSourceFishing fish = (ResourceSourceFishing)visibleObject;
            int i;
            for (i = 0; i < fishItems.Length; i++)
            {
                fishItems[i].gameObject.SetActive(false);
            }
            i = 0;
            foreach (RandomResourceOutputItem RRO in fish.Output)
            {
                if (i < animalResources.Length)
                {
                    fishItems[i].gameObject.SetActive(true);
                    fishItems[i].SetStats(RRO.OfferedResource.itemInfo.icon, RRO.Probability) ;
                    i++;
                }
            }
        }
    }
    
    private void FillVisitorPanels(Building Building)
    {
        List<Unit> Visitors = Building.CurrentVisitors;
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
