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
    public TMP_Text accuracyNumber;
    public TMP_Text dodgeNumber;
    public TMP_Text defenceNumber;
    public TMP_Text critNumber;
    public TMP_Text actionRestrictionTip;
    string viewUnitName = "";
    public List<(SkillUI, SkillType)> skillList;

    private void Awake()
    {
        skillList = new List<(SkillUI, SkillType)>();
        foreach (SkillType skillType in Enum.GetValues(typeof(SkillType)))
        {
            if (!skillType.ToString().Equals("none"))
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

        if (viewUnitName != visibleObject.objectName)
        {
            PanelControl.Instance.SetActivePanel(5);
        }

        if (visibleObject is UnitPlayer)
        {
            this.skillLayout.SetActive(true);
            this.actionRestrictionTip.transform.gameObject.SetActive(true);
            UnitPlayer unit = (UnitPlayer)visibleObject;
            this.UpdateCombatStats(unit);
            // player skills
            foreach (var skills in this.skillList)
            {
                skills.Item1.Show(unit, skills.Item2);
            }
        }
        else if (visibleObject is UnitAnimal)
        {
            this.skillLayout.SetActive(false);
            this.actionRestrictionTip.transform.gameObject.SetActive(false);
            UnitAnimal unit = (UnitAnimal)visibleObject;
            this.UpdateCombatStats(unit);
        }
        viewUnitName = visibleObject?.objectName;
    }

    private void UpdateCombatStats(Unit unit)
    {
        this.damageNumber.text = unit.BaseDamage.ToString();
        this.accuracyNumber.text = unit.Accuracy.ToString() + "%";
        this.defenceNumber.text = unit.Defence.ToString();
        this.critNumber.text = unit.CritChance.ToString() + "%";
        this.dodgeNumber.text = unit.Dodge.ToString() + "%";
    }
}
