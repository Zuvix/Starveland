using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CodeMonkey.Utils;

public class MouseEvents : Singleton<MouseEvents>
{
    public UnityEvent<GameObject, bool> viewObjectChanged=new UnityEvent<GameObject, bool>();
    public UnityEvent<GameObject, GameObject> OnSelectedObjectChanged = new UnityEvent<GameObject, GameObject>();
    private float mouseMoveTimer;
    private float mouseMoveTimerMax = .02f;
    private float objectChangedTimer=0f;
    private float objectChangedTimerMax = .2f;
    private GameObject viewedObject=null;

    private GameObject realSelectedObject = null;
    public GameObject selectedObject
    {
        get
        {
            return realSelectedObject;
        }
        private set
        {
            GameObject previousSelectedObject = realSelectedObject;
            realSelectedObject = value;
            OnSelectedObjectChanged.Invoke(realSelectedObject, previousSelectedObject);
        }
    }

    public GameObject frame;

    private BuildingSpecificPanel ActiveBuildingMenuPanel = null;

    private bool dragEnabled = true;
    public bool DragEnabled
    {
        get
        {
            return dragEnabled;
        }
        set
        {
            this.dragEnabled = value;
            frame.SetActive(value);
        }
    }
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
        
        if (ActiveBuildingMenuPanel == null)
        {
            if (GlobalGameState.Instance.InGameInputAllowed)
            {
                Vector3 MousePosition = UtilsClass.GetMouseWorldPosition();
                MapControl.Instance.map.GetXY(MousePosition, out int x, out int y);
                bool IsInMap = MapControl.Instance.map.IsInBounds(x, y);
                GameObject mapValue = GetSelectedGameObject(MousePosition);

                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log($"Left-clicked {x}, {y}. It is in map: {IsInMap}");
                    if (IsInMap)
                    {
                        //GameObject mapValue = MapControl.Instance.map.GetValue(UtilsClass.GetMouseWorldPosition());
                        
                        selectedObject = mapValue;
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    //Debug.LogWarning($"Right-clicked {x}, {y}. It is in map: {IsInMap}");
                    if (IsInMap)
                    {
                        if (mapValue != null)
                        {
                            mapValue.GetComponent<CellObject>().RightClickAction();
                        }
                    }
                }
            }
        }
        else if(DragEnabled && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            ActiveBuildingMenuPanel.Hide();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DragEnabled)
        {
            HandleMouseMove();
        }
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
    public void RegisterVisibleBuildingPanel(BuildingSpecificPanel ActivePanel)
    {
        this.ActiveBuildingMenuPanel = ActivePanel;
    }
    public void UnregisterVisibleBuildingPanel()
    {
        this.ActiveBuildingMenuPanel = null;
        this.DragEnabled = true;
    }

    public void SimulateClickOnObject(GameObject go)
    {
        this.viewedObject = go;
        this.selectedObject = go;
    }
}
