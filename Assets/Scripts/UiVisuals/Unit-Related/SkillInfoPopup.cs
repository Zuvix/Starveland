using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillInfoPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject SkillInfoPopupPanel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillInfoPopupPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillInfoPopupPanel.SetActive(false);
    }

    private void Awake()
    {
        SkillInfoPopupPanel.SetActive(false);
    }
}
