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
    private GameObject viewedObject=null;
    private void HandleMouseMove()
    {
        mouseMoveTimer -= Time.deltaTime;
        if (mouseMoveTimer < 0f)
        {
            mouseMoveTimer += mouseMoveTimerMax;
            GameObject mapValue = MapControl.Instance.map.GetValue(UtilsClass.GetMouseWorldPosition());
            Debug.Log(mapValue);
            if (!mapValue==(viewedObject))
            {
                viewedObject = mapValue;
                viewObjectChanged.Invoke(viewedObject);         
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseMove();
    }
}
