using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable_2 : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private bool Moving;
    private Vector3 OriginalPosition;
    private RectTransform m_transform;
    void Start()
    {
        Debug.Log("Initialized 2");
        Moving = false;
        m_transform = GetComponent<RectTransform>();
        OriginalPosition = new Vector3();
        /*OriginalPosition.x = m_transform.position.x;
        OriginalPosition.y = m_transform.position.y;
        OriginalPosition.z = m_transform.position.z;*/
    }

    public void OnDrag(PointerEventData eventData)
    {
            /*Debug.Log("Dragging 2");
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.gameObject.transform.localPosition = new Vector3(MousePosition.x - OriginalPosition.x, MousePosition.y - OriginalPosition.y, this.gameObject.transform.localPosition.z);*/
        Debug.Log("Dragging 2");

        Vector3 DeltaMovement = new Vector3(eventData.delta.x, eventData.delta.y);
        m_transform.position += DeltaMovement;
        OriginalPosition += DeltaMovement;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Grabbed 2");
        //Moving = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Moving = false;
        Debug.Log("Dropped 2");
        m_transform.position -= OriginalPosition;
    }
}