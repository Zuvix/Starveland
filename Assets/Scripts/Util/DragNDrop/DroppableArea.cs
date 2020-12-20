using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DroppableArea : MonoBehaviour, IPointerUpHandler
{
    public UnityEvent<UnitPanel> DroppedInArea = new UnityEvent<UnitPanel>();

    public void OnPointerUp(PointerEventData eventData)
    {
        DroppedInArea.Invoke(this.gameObject.GetComponent<UnitPanel>());
    }
}
