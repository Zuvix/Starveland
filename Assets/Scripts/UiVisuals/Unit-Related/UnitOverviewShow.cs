using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitOverviewShow : MonoBehaviour, IPointerClickHandler
{
    private UnitPlayer Unit;
    public Image normalIcon;
    public Sprite deathIcon;
    public Image SelectedImg;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.Unit != null && eventData.button == PointerEventData.InputButton.Left)
        {
            MouseEvents.Instance.SimulateClickOnObject(Unit.gameObject);
            UnitOverview.Instance.HidePanelHighlights();
            SelectedImg.enabled = true;
        }
    }

    public void SetUnit(UnitPlayer Unit)
    {
        SelectedImg.enabled = false;
        this.Unit = Unit;
        this.normalIcon.sprite = Unit.sr.sprite;
        Unit.onPlayerDeath.AddListener(ChangeIcon);
        Unit.onSpriteChange.AddListener(ChangeIcon);
    }

    private void ChangeIcon()
    {
        this.normalIcon.sprite = this.deathIcon;
    }

    private void ChangeIcon(Sprite sprite)
    {
        this.normalIcon.sprite = sprite;
    }
}
