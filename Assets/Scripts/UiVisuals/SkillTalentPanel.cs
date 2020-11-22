using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SkillTalentPanel : MonoBehaviour
{
    public GameObject skillTalentPanel;
    public GameObject skillHeader;
    public GameObject skillLayout;
    public TMP_Text damageNumber;
    public List<(SkillUI, SkillType)> skillList;
    public List<GameObject> UIs;

    private void Awake()
    {
        skillList = new List<(SkillUI, SkillType)>();
        foreach (SkillType skillType in Enum.GetValues(typeof(SkillType)))
        {
            if (!skillType.ToString().Equals("mining") && !skillType.ToString().Equals("none")) //todo remove mining when implemented
            {
                GameObject skillHeaderGO = Instantiate(skillHeader, skillLayout.gameObject.transform);
                SkillUI skillUI = skillHeaderGO.GetComponent<SkillUI>();
                skillUI.Show(null, skillType);
                skillList.Add((skillUI, skillType));
                skillHeaderGO.SetActive(true);
            }
        }
        skillTalentPanel.SetActive(false);
    }

    private void Start()
    {
        MouseEvents.Instance.viewObjectChanged.AddListener(UpdateSkillTalentPanel);
    }

    public void UpdateSkillTalentPanel(GameObject go, bool isSelected)
    {
        if (go == null || !isSelected)
        {
            this.skillTalentPanel.SetActive(false);
            this.UIs[0].SetActive(true); // panel at index 0 is default
            return;
        }

        CellObject visibleObject = go.GetComponent<CellObject>();

        if (visibleObject is UnitPlayer)
        {
            this.DeactivateOtherPanels();
            this.skillTalentPanel.SetActive(true);
            UnitPlayer unit = (UnitPlayer)visibleObject;
            // player unit stats
            this.damageNumber.text = unit.BaseDamage.ToString();
            // player skills
            foreach (var skills in this.skillList)
            {
                skills.Item1.Show(unit, skills.Item2);
            }

        }
    }

    private void DeactivateOtherPanels()
    {
        foreach (var ui in UIs)
        {
            ui.SetActive(false);
        }
    }
}
