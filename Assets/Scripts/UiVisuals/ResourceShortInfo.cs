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
    public void ShowInfo(List<Resource> resources)
    {
        HideAll();
        for (int i = 0; i < resources.Count; i++)
        {
            if (i < panels.Length)
            {
                panels[i].SetActive(true);
                icons[i].sprite = resources[i].itemInfo.icon;
                valueTxts[i].text = resources[i].Amount.ToString();
            }
        }
    }
    private void HideAll()
    {
        for(int i = 0; i < panels.Length;i++)
        {
            panels[i].SetActive(false);
        }
    }
}
