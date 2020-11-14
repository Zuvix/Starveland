using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Draggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    RectTransform m_transform = null;
    public Vector3 OriginalPosition;

    // Use this for initialization
    void Start()
    {
        m_transform = GetComponent<RectTransform>();
        OriginalPosition = new Vector3();
    }
    public void OnDrag(PointerEventData eventData)
    {
        m_transform.position += new Vector3(eventData.delta.x, eventData.delta.y);
        OriginalPosition += new Vector3(eventData.delta.x, eventData.delta.y);
        /* Debug.Log(OriginalPosition);
         Debug.Log(m_transform.position);*/
        // magic : add zone clamping if's here.
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogError("Pressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.LogError("Released");
        m_transform.position -= OriginalPosition;
    }
}