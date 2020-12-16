using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuildingInfoPopupPanel : MonoBehaviour
{
    public TMP_Text BuildingNameLabel;
    public TMP_Text BuildingDescriptionLabel;
    public List<GameObject> ResourcePanels;
    private List<Resource> Cost;
    //private Vector3 PositionBackup;

    private static readonly Color DefaultColor = new Color32(45, 60, 91, 180);
    private static readonly Color UnavailableColor = new Color(0.9811f, 0.3831f, 0.3831f, 0.533f);
    void Awake()
    {
        this.gameObject.SetActive(false);
    }
    private void Start()
    {
        DaytimeCounter.Instance.OnDayOver.AddListener(Hide);
    }

    public void Display(string Name, string Description, List<Resource> Cost)
    {
        Debug.LogWarning("BuildingInfoPopupPanel::Display");
        BuildingNameLabel.text = Name;
        BuildingDescriptionLabel.text = Description;
        this.Cost = Cost;
        DisplayResourcePanels();

        GlobalInventory.Instance.OnInventoryUpdate.AddListener(DisplayResourcePanels);

        //this.PositionBackup = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        Debug.LogWarning("BuildingInfoPopupPanel::Hide");
        GlobalInventory.Instance.OnInventoryUpdate.RemoveListener(DisplayResourcePanels);
        this.gameObject.SetActive(false);
        //this.gameObject.GetComponent<RectTransform>().anchoredPosition = this.PositionBackup;
    }
    private void DisplayResourcePanels()
    {
        Debug.LogWarning("BuildingInfoPopupPanel::DisplayResourcePanels");
        for (int i = 0; i < Cost.Count && i < ResourcePanels.Count; i++)
        {
            ResourcePanels[i].GetComponent<BuildingInfoResourcePanel>().Fill(Cost[i]);
            CheckResourceAvailability(Cost[i], ResourcePanels[i]);
            ResourcePanels[i].SetActive(true);
        }
        for (int i = Cost.Count; i < ResourcePanels.Count; i++)
        {
            ResourcePanels[i].SetActive(false);
        }
    }
    private void CheckResourceAvailability(Resource Resource, GameObject ResourcePanel)
    {
        if (GlobalInventory.Instance.CheckAvaliableItem(Resource.itemInfo.name, Resource.Amount))
        {
            ResourcePanel.GetComponent<Image>().color = DefaultColor;
        }
        else
        {
            ResourcePanel.GetComponent<Image>().color = UnavailableColor;
        }
    }
}