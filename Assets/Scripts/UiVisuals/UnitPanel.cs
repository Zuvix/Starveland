using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    public Slider Slider;
    public UnitHungry Unit;

    public void UpdateSlider(float amount)
    {
        Slider.GetComponent<ProgressBar>().CurrentProgress = amount;
    }
}