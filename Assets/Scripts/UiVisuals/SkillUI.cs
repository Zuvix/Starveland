using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public TMP_Text skillName;
    public TMP_Text skillLevel;
    public Image skillIcon;
    public Slider experienceBar;
    public GameObject TalentsLayout;
    public GameObject TalentsUI;
    private List<(string, TalentsUI, GameObject)> talentUIsList;
    private int LastUnitID;

    public void Show(UnitPlayer Unit, SkillType skillType)
    {
        if (Unit != null)
        {
            this.skillLevel.text = $"level {Unit.Skills[skillType].Level}";
            this.experienceBar.GetComponent<ExperienceBar>().CurrentProgress = Unit.Skills[skillType].CurrentExperience;
            this.skillIcon.sprite = Unit.Skills[skillType].icon;

            // if the show is called when the same unit is selected we don't need to destroy talentUI, only update it
            if (Unit.gameObject.GetInstanceID() != LastUnitID)
            {
                foreach (var talentUI in talentUIsList)
                {
                    Destroy(talentUI.Item3);
                }
                talentUIsList.Clear();
            }

            // show every talent of the selected unit
            foreach (var talent in Unit.Skills[skillType].SkillAppliedTalents)
            {
                this.ShowTalents(talent);
            }

            // hunting has talents on the unit itself, so we need to iterate different list
            if (skillType.Equals(SkillType.Hunting))
            {
                foreach (var talent in Unit.UnitAppliedTalents)
                {
                    this.ShowTalents(talent);
                }
            }

            this.LastUnitID = Unit.gameObject.GetInstanceID();
            return;
        }
        // this should by called only once during inicialization
        this.skillName.text = $"{skillType.ToString()}:";
        this.talentUIsList = new List<(string, TalentsUI, GameObject)>();
    }

    private void ShowTalents(Talent talent)
    {
        bool exists = false;
        TalentsUI talentUI = null;
        foreach (var tui in talentUIsList)
        {
            if (talent.Name.Equals(tui.Item1))
            {
                exists = true;
                talentUI = tui.Item2;
                break;
            }
        }
        if (!exists)
        {
            GameObject talentsUIgo = Instantiate(TalentsUI, TalentsLayout.transform);
            talentsUIgo.SetActive(true);
            talentUI = talentsUIgo.GetComponent<TalentsUI>();
            talentUIsList.Add((talent.Name, talentUI, talentsUIgo));
        }
        talentUI.Show(talent);
    }
}
