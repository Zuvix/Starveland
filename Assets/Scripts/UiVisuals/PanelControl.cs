using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : Singleton<PanelControl>
{
    private int activePanel=0;
    public GameObject[] Panels;
    public void DeactivatePanels()
    {
        foreach (GameObject panel in Panels)
        {
            panel.SetActive(false);
        }
    }
    public int GetActivePanelID()
    {
        return activePanel;
    }
    public void SetActivePanel(int i)
    {
        DeactivatePanels();
        Panels[i].SetActive(true);
        activePanel = i;
    }
}
