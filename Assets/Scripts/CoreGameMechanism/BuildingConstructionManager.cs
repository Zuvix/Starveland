using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstructionManager : Singleton<BuildingConstructionManager>
{
    
    private GameObject CurrentlySelectedBuilding = null;

    private GameObject CurrentBackground;
    private GameObject BuildingMock = null;
    private MapCell LastCellPointedOn = null;
    private bool LastCellHasBuildingMock = false;
    void Awake()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("A building is selected");
        if (Input.GetMouseButtonDown(0))
        {
            if (LastCellHasBuildingMock)
            {
                MapCell CellToCreateBuildingOn = LastCellPointedOn;
                GameObject BuildingToCreate = CurrentlySelectedBuilding;

                DeselectBuilding();

                MapControl.Instance.CreateGameObject(CellToCreateBuildingOn.x, CellToCreateBuildingOn.y, BuildingToCreate);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("The building has been deselected");
            DeselectBuilding();
        }
        else
        {
            DestroyBuildingMock();

            Vector3 MousePosition = UtilsClass.GetMouseWorldPosition();
            MapControl.Instance.map.GetXY(MousePosition, out int x, out int y);
            bool IsInMap = MapControl.Instance.map.IsInBounds(x, y);
            if (IsInMap)
            {
                LastCellPointedOn = MapControl.Instance.map.Grid[x][y];

                if (LastCellPointedOn.CanBeEnteredByObject(CurrentlySelectedBuilding.GetComponent<Building>().IsBlocking))
                {
                    //Debug.LogError(MapControl.Instance.GreenBackground);
                    CurrentBackground = Instantiate(MapControl.Instance.GreenBackground);
                    MapControl.Instance.map.CenterObject(x, y, CurrentBackground);

                    BuildingMock = MapControl.Instance.CreateGameObject(x, y, CurrentlySelectedBuilding);
                    LastCellHasBuildingMock = true;
                }
            }
        }
    }
    public void SelectBuilding(GameObject Building)
    {
        this.CurrentlySelectedBuilding = Building;
        this.gameObject.SetActive(true);
    }
    public void DeselectBuilding()
    {
        DestroyBuildingMock();

        CurrentlySelectedBuilding = null;
        BuildingMock = null;
        LastCellPointedOn = null;
        LastCellHasBuildingMock = false;

        this.gameObject.SetActive(false);
        
    }
    private void DestroyBuildingMock()
    {
        if (LastCellPointedOn != null && LastCellHasBuildingMock)
        {
            LastCellPointedOn.EraseCellObject();
            Destroy(BuildingMock);
            BuildingMock = null;
            LastCellHasBuildingMock = false;

            Destroy(CurrentBackground);
            CurrentBackground = null;
        }
    }
}
