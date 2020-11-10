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

    List<GameObject> contentPanels;
    private void Awake()
    {
        contentPanels = new List<GameObject>()
        {
            topContent,
            unitContent
        };
        HideTopContent();

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
    public void UpdateTextInfo(GameObject go)
    {
        HideTopContent();
        if (go==null)
        {
            HideTopContent();
            return;
        }
        if (go.GetComponent<CellObject>() != null)
        {
            topContent.SetActive(true);
            CellObject co = go.GetComponent<CellObject>();
            img.sprite = co.sr.sprite;
            nameTxt.text = co.objectName;
        }
        if (go.GetComponent<Unit>() != null)
        {
            unitContent.SetActive(true);
            Unit unit = go.GetComponent<Unit>();
            unitHP.text = "TODO";
            unitAction.text = "TODO";
            tipTxt.text = unit.tip;
        }
        if (go.GetComponent<ResourceSource>()!=null)
        {

        }
    }
}
