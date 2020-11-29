using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UnitPanel : MonoBehaviour
{
    public Slider Slider;
    public TMP_Text fedAmountTxt;
    public UnitHungry Unit;

    public void UpdateSlider(float amount)
    {
        Slider.GetComponent<ProgressBar>().CurrentProgress = amount;
        fedAmountTxt.SetText(amount*10 + "/10");
    }
}