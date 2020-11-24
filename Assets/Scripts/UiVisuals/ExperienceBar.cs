using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ExperienceBar : MonoBehaviour
{
    public Slider Slider;
    public GameObject Fill;
    public TMP_Text experienceText;
    private int currentProgress = 0;
    private int maxProgress;
    public int CurrentProgress
    {
        get
        {
            return currentProgress;
        }
        set
        {
            this.experienceText.text = $"{value % maxProgress}/{maxProgress}";
            currentProgress = Math.Min(value % maxProgress, maxProgress);
            Slider.value = (float)currentProgress / (float)maxProgress;
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
    void Awake()
    {
        this.maxProgress = GameConfigManager.Instance.GameConfig.ExperienceNeededToLevelUp;
        this.experienceText.text = $"{0}/{maxProgress}";
        Fill.SetActive(false);
        Slider.value = 0f;
    }
}
