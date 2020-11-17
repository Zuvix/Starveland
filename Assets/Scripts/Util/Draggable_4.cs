﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable_4 : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Color OriginalColour;
    public void OnDrag(PointerEventData eventData)
    {
        FeedingManager.Instance.DraggedObject.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        FeedingManager.Instance.DraggedObject.SetActive(true);
        OriginalColour = this.GetComponent<Image>().color;
        this.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        FeedingManager.Instance.DraggedObject.SetActive(false);
        this.GetComponent<Image>().color = OriginalColour;
    }
}