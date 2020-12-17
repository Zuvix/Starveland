using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIReference : Singleton<GUIReference>
{
    public GameObject ShipProgressPopupPanel;
    public List<Image> ShipProgressMaterialIcons;
    public List<TMP_Text> ShipProgressMaterialLabels;
}
