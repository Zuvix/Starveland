using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitOverview : MonoBehaviour
{
    public GameObject UnitOverviewPanel;
    public GameObject UnitPanel;

    private void Awake()
    {
        this.UnitOverviewPanel.SetActive(true);
    }

    private void Start()
    {
        foreach (UnitPlayer unit in Unit.PlayerUnitPool)
        {
            var go = Instantiate(UnitPanel, UnitOverviewPanel.gameObject.transform);
            go.SetActive(true);
            go.GetComponentInChildren<UnitOverviewShow>().SetUnit(unit);
        }
    }
}
