﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CancelCurrentActionButton : MonoBehaviour, IPointerDownHandler
{
    public void Start()
    {
        MouseEvents.Instance.OnSelectedObjectChanged.AddListener(OnNewObjectSelected);
        this.gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            MouseEvents.Instance.selectedObject.GetComponent<UnitPlayer>().SetActivity(new ActivityStateIdle());
        }
    }
    private void OnNewObjectSelected(GameObject CurrentlySelectedObject, GameObject PreviouslySelectedObject)
    {
        if (PreviouslySelectedObject != null)
        {
            PreviouslySelectedObject.GetComponent<UnitPlayer>().OnActivityStateChanged.RemoveListener(ToggleVisibility);
        }

        if (CurrentlySelectedObject == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        UnitPlayer UnitPlayerComponent = CurrentlySelectedObject.GetComponent<UnitPlayer>();
        UnitPlayerComponent.OnActivityStateChanged.AddListener(ToggleVisibility);
        if (UnitPlayerComponent != null)
        {
            ToggleVisibility(UnitPlayerComponent.CurrentActivity);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    private void ToggleVisibility(ActivityState ActivityState)
    {
        this.gameObject.SetActive(ActivityState.IsCancellable());
    }
}
