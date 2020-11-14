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
    private void HandleMouseMove()
    {
        mouseMoveTimer -= Time.deltaTime;
        objectChangedTimer -= Time.deltaTime;
        if (mouseMoveTimer < 0f)
        {
            mouseMoveTimer += mouseMoveTimerMax;
            GameObject mapValue = MapControl.Instance.map.GetValue(UtilsClass.GetMouseWorldPosition());
            if (mapValue != null)
            {
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
                if (viewedObject != null)
                {
                    viewObjectChanged.Invoke(null);
                    objectChangedTimer = objectChangedTimerMax;
                    viewedObject = null;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseMove();
    }
}
