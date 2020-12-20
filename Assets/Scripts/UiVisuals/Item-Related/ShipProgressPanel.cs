using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipProgressPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GUIReference.Instance.ShipProgressPopupPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GUIReference.Instance.ShipProgressPopupPanel.SetActive(false);
    }
}
