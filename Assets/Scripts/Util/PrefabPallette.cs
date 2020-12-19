using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PrefabPallette : Singleton<PrefabPallette>
{
    public Camera Camera;
    public GameObject Canvas;

    public Sprite VoidSprite;

    public GameObject CellObjectSliderPrefab;

    public GameObject GenericBuildingMock;

    public TMP_Text ShipProgressPerecentLabel;
}
