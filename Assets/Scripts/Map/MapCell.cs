using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell
{
    public static readonly bool BLOCKING = true;
    public static readonly bool NONBLOCKING = false;
    public int x { get; }
    public int y { get; }
    public CellObject CurrentObject { get; private set; } = null;
    public Unit CurrentUnit { get; private set; } = null;
    public Map Map { get; }

    public Vector3 position;
    public MapCell(Vector3 position, Map Map, int x, int y)
    {
        this.position = position;
        this.Map = Map;
        this.x = x;
        this.y = y;
    }
    public CellObject GetTopSelectableObject()
    {
        CellObject Result = null;
        if (this.CurrentUnit != null)
        {
            Result = this.CurrentUnit;
        }
        else if (this.CurrentObject != null && this.CurrentObject.IsSelectable)
        {
            Result = this.CurrentObject;
        }
        return Result;
    }
    public bool SetCellObject(CellObject CellObject)
    {
        bool Success = CanBeEnteredByObject(CellObject.IsBlocking);
        if (Success)
        {
            EraseCellObject();
            CellObject.SetCurrentCell(this);
            this.CurrentObject = CellObject;
        }
        return Success;
    }
    public bool SetUnit(Unit Unit)
    {
        bool Success = CanBeEnteredByUnit();
        if (Success)
        {
            EraseUnit();
            Unit.SetCurrentCell(this);
            this.CurrentUnit = Unit;
        }
        return Success;
    }
    public void EraseCellObject()
    {
        if (this.CurrentObject != null)
        {
            CellObject.Destroy(this.CurrentObject.gameObject);
        }
        this.CurrentObject = null;
    }
    public void EraseUnit()
    {
        this.CurrentUnit = null;
    }
    public bool CanBeEnteredByObject(bool EnteringObjectIsBlocking)
    {
        return (this.CurrentObject == null && (this.CurrentUnit == null || !EnteringObjectIsBlocking)) || (this.CurrentObject!=null && !this.CurrentObject.IsBlocking && EnteringObjectIsBlocking && this.CurrentUnit==null);
    }
    public bool CanBeEnteredByUnit()
    {
        return this.CurrentUnit == null && (this.CurrentObject == null || !this.CurrentObject.IsBlocking);
    }
    public void RespondToActionOrder()
    {
        //Debug.LogWarning($"{this} ({x},{y})responds to right click. Current Unit is {this.CurrentUnit}, current Object is {this.CurrentObject}");

        if (this.CurrentUnit != null && this.CurrentUnit.IsPossibleToAddToActionQueue)
        {
            //Debug.LogWarning($"{CurrentUnit} responds to right click");
            CurrentUnit.AddToActionQueue();
        }
        else if (this.CurrentObject != null && this.CurrentObject.IsPossibleToAddToActionQueue)
        {
            //Debug.LogWarning($"{CurrentObject} responds to right click");
            CurrentObject.AddToActionQueue();
        }
        //Debug.LogWarning($"No response to right click");
    }
    public virtual PathSearchNode ProducePathSearchNode(List<List<PathSearchNode>> Map)
    {
        return new PathSearchNode(this, Map);
    }

    public List<MapCell> GetNeighbours()
    {
        List<MapCell> Neighbours = new List<MapCell>();
        if (this.x > 0)
        {
            Neighbours.Add(this.Map.Grid[this.x - 1][this.y]);
        }
        if (this.x < this.Map.Grid.Count - 1)
        {
            Neighbours.Add(this.Map.Grid[this.x + 1][this.y]);
        }
        if (this.y > 0)
        {
            Neighbours.Add(this.Map.Grid[this.x][this.y - 1]);
        }
        if (this.y < this.Map.Grid[0].Count - 1)
        {
            Neighbours.Add(this.Map.Grid[this.x][this.y + 1]);
        }

        return Neighbours;
    }
}
