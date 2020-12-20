using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuildingInfoResourcePanel : MonoBehaviour
{
    public GameObject ResourceIcon;
    public TMP_Text ResourceLabel;

    public void Fill(Resource Resource)
    {
        ResourceIcon.GetComponent<Image>().sprite = Resource.itemInfo.icon;
        ResourceLabel.text = $"{Resource.Amount} {Resource.itemInfo.name}";

        this.gameObject.SetActive(true);
    }
}
