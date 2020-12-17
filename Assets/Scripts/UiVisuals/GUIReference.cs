using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIReference : Singleton<GUIReference>
{
    // Scene indices
    public readonly int SCENE_INDEX_GAMEOVER = 2;


    // Ship-related GUI
    [Header("Ship Progress")]
    public GameObject ShipProgressPopupPanel;
    public List<Image> ShipProgressMaterialIcons;
    public List<TMP_Text> ShipProgressMaterialLabels;
}
