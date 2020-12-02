using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BuildingSpecificPanelCloseButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject SuperPanel;
    public void OnPointerClick(PointerEventData eventData)
    {
        SuperPanel.GetComponent<BuildingSpecificPanel>().Hide();
    }
}
