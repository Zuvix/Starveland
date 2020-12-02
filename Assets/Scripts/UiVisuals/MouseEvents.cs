using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CodeMonkey.Utils;

public class MouseEvents : Singleton<MouseEvents>
{
    public UnityEvent<GameObject, bool> viewObjectChanged=new UnityEvent<GameObject, bool>();
    private float mouseMoveTimer;
    private float mouseMoveTimerMax = .02f;
    private float objectChangedTimer=0f;
    private float objectChangedTimerMax = .2f;
    private GameObject viewedObject=null;
    private GameObject selectedObject = null;
    public GameObject frame;
    private void HandleMouseMove()
    {
        mouseMoveTimer -= Time.deltaTime;
        objectChangedTimer -= Time.deltaTime;
        if (mouseMoveTimer < 0f)
        {
            frame.SetActive(true);
            mouseMoveTimer += mouseMoveTimerMax;

            Vector3 MousePosition = UtilsClass.GetMouseWorldPosition();
            GameObject mapValue = GetSelectedGameObject(MousePosition);

            //error
            //GameObject mapValue = MapControl.Instance.map.GetValue(MousePosition);
            //object coordinates


            if (selectedObject != null)
            {
                frame.transform.position = selectedObject.transform.position;
                if (objectChangedTimer < 0f)
                {
                    viewObjectChanged.Invoke(selectedObject, true);
                    objectChangedTimer = objectChangedTimerMax;
                }
            }
            else if (mapValue != null)
            {
                frame.transform.position = mapValue.transform.position;
                if (!mapValue.Equals(viewedObject))
                {
                    viewedObject = mapValue;
                    viewObjectChanged.Invoke(viewedObject, false);
                    objectChangedTimer = objectChangedTimerMax;
                }
                if (objectChangedTimer < 0f)
                {
                    viewObjectChanged.Invoke(viewedObject, false);
                    objectChangedTimer = objectChangedTimerMax;
                }
            }
            else
            {
                MapControl.Instance.map.GetXY(MousePosition, out int x, out int y);
                MapControl.Instance.map.CenterObject(x, y, frame);
                if (!MapControl.Instance.map.IsInBounds(x, y))
                {
                    frame.SetActive(false);
                }
                if (viewedObject != null)
                {
                    viewObjectChanged.Invoke(null, false);
                    objectChangedTimer = objectChangedTimerMax;
                    viewedObject = null;
                }

            }
        }
    }
    public void HandleMouseClick()
    {
        if (GlobalGameState.Instance.InGameInputAllowed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 MousePosition = UtilsClass.GetMouseWorldPosition();
                MapControl.Instance.map.GetXY(MousePosition, out int x, out int y);
                bool IsInMap = MapControl.Instance.map.IsInBounds(x, y);
                Debug.Log($"Clicked {x}, {y}. It is in map: {IsInMap}");
                if (IsInMap)
                {
                    //GameObject mapValue = MapControl.Instance.map.GetValue(UtilsClass.GetMouseWorldPosition());
                    GameObject mapValue = GetSelectedGameObject(MousePosition);
                    selectedObject = mapValue;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Vector3 MousePosition = UtilsClass.GetMouseWorldPosition();
                MapControl.Instance.map.GetXY(MousePosition, out int x, out int y);
                bool IsInMap = MapControl.Instance.map.IsInBounds(x, y);
                Debug.LogWarning($"Right-clicked {x}, {y}. It is in map: {IsInMap}");
                if (IsInMap)
                {
                    MapControl.Instance.map.Grid[x][y].RespondToActionOrder();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseMove();
        HandleMouseClick();
    }

    private GameObject GetSelectedGameObject(Vector3 MousePosition)
    {
        MapControl.Instance.map.GetXY(MousePosition, out int x, out int y);
        bool IsInMap = MapControl.Instance.map.IsInBounds(x, y);
        GameObject Result = null;
        if (IsInMap)
        {
            CellObject TopSelectableObject = MapControl.Instance.map.Grid[x][y].GetTopSelectableObject();
            Result = TopSelectableObject == null ? null : TopSelectableObject.gameObject;
        }

        return Result;
    }
}
