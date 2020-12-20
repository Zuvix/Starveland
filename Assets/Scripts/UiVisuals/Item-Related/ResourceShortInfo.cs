using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResourceShortInfo : MonoBehaviour
{
    public TMP_Text[] valueTxts;
    public Image[] icons;
    public GameObject[] panels;
    private void Awake()
    {
        HideAll();
    }
    public void ShowInfo(Resource resource)
    {
        HideAll();
        panels[0].SetActive(true);
        icons[0].sprite = resource.itemInfo.icon;
        valueTxts[0].text = resource.Amount.ToString();
        
    }
    private void HideAll()
    {
        for(int i = 0; i < panels.Length;i++)
        {
            panels[i].SetActive(false);
        }
    }
}
