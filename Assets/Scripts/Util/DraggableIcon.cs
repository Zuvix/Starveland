using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableIcon : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Color OriginalColour;
    public Resource Resource;

    // https://dev.to/matthewodle/simple-ui-element-dragging-script-in-unity-c-450p
    public void OnDrag(PointerEventData eventData)
    {
        FeedingManager.Instance.DraggedObject.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        FeedingManager.Instance.DraggedObject.SetActive(true);
        OriginalColour = this.GetComponent<Image>().color;
        this.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        FeedingManager.Instance.DraggedObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
        FeedingManager.Instance.SelectedFoodIcon = this.gameObject;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        FeedingManager.Instance.DraggedObject.SetActive(false);
        this.GetComponent<Image>().color = OriginalColour;

        // https://answers.unity.com/questions/884262/catch-pointer-events-by-multiple-gameobjects.html
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (RaycastResult raycastResult in raycastResults)
        {
            GameObject newTarget = raycastResult.gameObject; //Array item 1 should be the one next underneath, handy to implement for-loop with check here if necessary.
            //print($"Passing on click to {newTarget}"); //Just make sure you caught the right object
            if (FeedingManager.Instance.UnitPanels.Contains(newTarget))
            {
                ExecuteEvents.Execute(newTarget, eventData, ExecuteEvents.pointerUpHandler);
            }
        }

        FeedingManager.Instance.SelectedFoodIcon = null;
    }
}