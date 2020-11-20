using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
{
    public Slider Slider;
    public GameObject Fill;
    private float currentProgress = 0f;
    private float maxProgress = 1f;
    public float CurrentProgress
    {
        get
        {
            return currentProgress;
        }
        set
        {
            currentProgress = Math.Min(value, maxProgress);
            Slider.value = currentProgress;
            if (currentProgress > 0)
            {
                Fill.SetActive(true);
            }
            else
            {
                Fill.SetActive(false);
            }
        }
    }
    void Start()
    {
        Fill.SetActive(false);
        Slider.value = 0;
    }
}
