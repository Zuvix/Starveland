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
    string viewUnitName = "";
    public List<(SkillUI, SkillType)> skillList;

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
    }

    private void Start()
    {
        MouseEvents.Instance.viewObjectChanged.AddListener(UpdateSkillTalentPanel);
        skillTalentPanel.SetActive(false);
    }

    public void UpdateSkillTalentPanel(GameObject go, bool isSelected)
    {
        if (go == null || !isSelected)
        {
            //PanelControl.Instance.SetActivePanel(0);
            return;
        }

        CellObject visibleObject = go.GetComponent<CellObject>();
        if (visibleObject is UnitPlayer)
        {
            if (viewUnitName != visibleObject.objectName)
            {
                PanelControl.Instance.SetActivePanel(5);
            }
            UnitPlayer unit = (UnitPlayer)visibleObject;
            // player unit stats
            this.damageNumber.text = unit.BaseDamage.ToString();
            // player skills
            foreach (var skills in this.skillList)
            {
                skills.Item1.Show(unit, skills.Item2);
            }
        }
        viewUnitName = visibleObject?.objectName;
    }
}
