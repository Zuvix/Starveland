﻿using CodeMonkey.Utils;
using System.Linq;
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
    private void Start()
    {
        DaytimeCounter.Instance.OnDayOver.AddListener(DeselectBuilding);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(MouseButton.Left))
        {
            PlaceBuilding();
        }
        else if (Input.GetMouseButtonDown(MouseButton.Right))
        {
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
                if (LastCellPointedOn != null)
                {
                    LastCellPointedOn.RestoreCellObject();
                }
                LastCellPointedOn = MapControl.Instance.map.Grid[x][y];

                if (LastCellPointedOn.CanBeEnteredByObject(CurrentlySelectedBuilding.GetComponent<Building>().IsBlocking))
                {
                    LastCellPointedOn.BackupCellObject();

                    CurrentBackground = Instantiate(MapControl.Instance.GreenBackground);
                    MapControl.Instance.map.CenterObject(x, y, CurrentBackground);

                    BuildingMock = MapControl.Instance.CreateGameObject(x, y, PrefabPallette.Instance.GenericBuildingMock);
                    BuildingMock.GetComponent<SpriteRenderer>().sprite = this.CurrentlySelectedBuilding.GetComponent<SpriteRenderer>().sprite;
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
        if (LastCellPointedOn != null)
        {
            LastCellPointedOn.RestoreCellObject();
        }
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
    private void PlaceBuilding()
    {
        if (LastCellHasBuildingMock)
        {
            MapCell CellToCreateBuildingOn = LastCellPointedOn;
            GameObject BuildingToCreate = CurrentlySelectedBuilding;

            DeselectBuilding();

            if (GlobalInventory.Instance.AttemptRemoveItems(BuildingToCreate.GetComponent<Building>().ConstructionCost))
            {
                MapControl.Instance.CreateGameObject(CellToCreateBuildingOn.x, CellToCreateBuildingOn.y, BuildingToCreate);

                CellToCreateBuildingOn.CurrentObject.CreatePopups(
                    BuildingToCreate.GetComponent<Building>().ConstructionCost.Select(res => (res.itemInfo.icon, -res.Amount)).ToList()
                );
            }
        }
    }
}