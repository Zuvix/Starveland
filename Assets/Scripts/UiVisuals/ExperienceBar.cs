using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Slider Slider;
    public GameObject Fill;
    public TMP_Text experienceText;
    private int[] maxProgress;
    private int maxLevel;
    private int currentProgress = 0;

    void Awake()
    {
        this.maxProgress = GameConfigManager.Instance.GameConfig.ExperienceToLevelUpForEachLevel;
        this.maxLevel = GameConfigManager.Instance.GameConfig.MaximumLevelOfSkills;
        this.experienceText.text = $"{0}/{maxProgress[0]}";
        Fill.SetActive(false);
        Slider.value = 0f;
    }

    public void SetProgress(int value, int level)
    {
        if (level >= this.maxLevel)
        {
            this.experienceText.text = $"Max";
            currentProgress = maxProgress[level - 1];
        }
        else
        {
            this.experienceText.text = $"{value}/{maxProgress[level - 1]}";
            currentProgress = Math.Min(value, maxProgress[level - 1]);
        }
        Slider.value = (float)currentProgress / (float)maxProgress[level - 1];
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
