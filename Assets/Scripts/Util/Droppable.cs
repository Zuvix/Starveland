﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Droppable : MonoBehaviour, IPointerUpHandler
{
    public UnityEvent DroppedInArea = new UnityEvent();

    public void OnPointerUp(PointerEventData eventData)
    {
        DroppedInArea.Invoke();
    }
}