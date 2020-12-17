using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitOverview : Singleton<UnitOverview>
{
    public GameObject UnitOverviewPanel;
    public GameObject UnitPanel;
    private List<Image> UnitPanels;

    private void Awake()
    {
        this.UnitOverviewPanel.SetActive(true);
        UnitPanels=new List<Image>();
    }

    public void HidePanelHighlights()
    {
        foreach(Image img in UnitPanels)
        {
            img.enabled = false;
        }
    }
    private void Start()
    {
        foreach (UnitPlayer unit in UnitManager.Instance.PlayerUnitPool)
        {
            var go = Instantiate(UnitPanel, UnitOverviewPanel.gameObject.transform);
            go.SetActive(true);
            UnitOverviewShow UOS;
            UOS = go.GetComponentInChildren<UnitOverviewShow>();
            UOS.SetUnit(unit);
            UnitPanels.Add(UOS.SelectedImg);
        }

    }
}
