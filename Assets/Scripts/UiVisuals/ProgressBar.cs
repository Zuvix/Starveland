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
            Debug.Log($"Slider values: {currentProgress}, {Slider.value}");
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
        Debug.Log("Progress bar start called");
        Fill.SetActive(false);
        Slider.value = 0;
    }
}
