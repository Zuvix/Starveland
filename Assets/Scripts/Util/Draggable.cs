using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Draggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    RectTransform m_transform = null;
    public Vector3 OriginalPosition;

    public readonly float IconMovementSpeedFactor = 0.65f;

    // Use this for initialization
    void Start()
    {
        m_transform = GetComponent<RectTransform>();
        OriginalPosition = new Vector3();
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 DeltaMovement = new Vector3(eventData.delta.x * IconMovementSpeedFactor, eventData.delta.y * IconMovementSpeedFactor);
        m_transform.position += DeltaMovement;
        OriginalPosition += DeltaMovement;
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