using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionConversion
{
    public static void MoveUIObjectToWorldObjectPosition(Vector3 WorldObjectPosition, GameObject UIObject, GameObject Canvas, Camera Cam)
    {
        RectTransform CanvasRect = Canvas.GetComponent<RectTransform>();
        RectTransform UI_Element = UIObject.GetComponent<RectTransform>();
        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = Cam.WorldToViewportPoint(WorldObjectPosition);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        UI_Element.anchoredPosition = WorldObject_ScreenPosition;
    }
}
