using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishItemUi : MonoBehaviour
{
    public TMP_Text percentTxt;
    public Image icon;
    public void SetStats(Sprite sprite,int chance)
    {
        icon.sprite = sprite;
        percentTxt.text = chance + "%";
    }
}
