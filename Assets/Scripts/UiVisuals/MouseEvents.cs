using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CodeMonkey.Utils;

public class MouseEvents : Singleton<MouseEvents>
{
    public UnityEvent<GameObject> viewObjectChanged=new UnityEvent<GameObject>();
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
            mouseMoveTimer += mouseMoveTimerMax;
            GameObject mapValue = MapControl.Instance.map.GetValue(UtilsClass.GetMouseWorldPosition());
            //object coordinates
            int x, y;
            MapControl.Instance.map.GetXY(UtilsClass.GetMouseWorldPosition(), out x, out y);

            if (selectedObject != null)
            {
                frame.transform.position = selectedObject.transform.position;
                if (objectChangedTimer < 0f)
                {
                    viewObjectChanged.Invoke(selectedObject);
                    objectChangedTimer = objectChangedTimerMax;
                }
            }
            else if (mapValue != null)
            {
                frame.transform.position = mapValue.transform.position;
                if (!mapValue.Equals(viewedObject))
                {
                    viewedObject = mapValue;
                    viewObjectChanged.Invoke(viewedObject);
                    objectChangedTimer = objectChangedTimerMax;
                }
                if (objectChangedTimer < 0f)
                {
                    viewObjectChanged.Invoke(viewedObject);
                    objectChangedTimer = objectChangedTimerMax;
                }
            }
            else
            {
                MapControl.Instance.map.CenterObject(x, y, frame);
                if (viewedObject != null)
                {
                    viewObjectChanged.Invoke(null);
                    objectChangedTimer = objectChangedTimerMax;
                    viewedObject = null;
                }

            }
        }
    }
    public void HandleMouseClick()
    {
        if(GlobalGameState.Instance.InGameInputAllowed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject mapValue = MapControl.Instance.map.GetValue(UtilsClass.GetMouseWorldPosition());
                selectedObject = mapValue;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseMove();
        HandleMouseClick();
    }
}
