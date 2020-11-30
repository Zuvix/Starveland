using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TalentsUI : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public TMP_Text talentEffect;
    public Image talentIcon;
    private string TextToShow;

    public void Show(Talent Talent)
    {
        //this.talentEffect.text = $"{Talent.Name} +{Talent.effect}%";
        this.TextToShow = $"{Talent.Description} +{Talent.Effect}%";
        this.talentIcon.sprite = Talent.icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.talentEffect.text = this.TextToShow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.talentEffect.text = "";
    }
}
