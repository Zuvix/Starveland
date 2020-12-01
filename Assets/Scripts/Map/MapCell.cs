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
            Unit.SetCurrentCell(this);
            this.CurrentUnit = Unit;
        }
        return Success;
    }
    public void EraseCellObject()
    {
        this.CurrentObject = null;
    }
    public void EraseUnit()
    {
        this.CurrentUnit = null;
    }
    public bool CanBeEnteredByObject(bool EnteringObjectIsBlocking)
    {
        return (this.CurrentObject == null && (this.CurrentUnit == null || !EnteringObjectIsBlocking)) || (this.CurrentObject!=null && !this.CurrentObject.IsBlocking && EnteringObjectIsBlocking);
    }
    public bool CanBeEnteredByUnit()
    {
        return this.CurrentUnit == null && (this.CurrentObject == null || !this.CurrentObject.IsBlocking);
    }
    public virtual PathSearchNode ProducePathSearchNode(List<List<PathSearchNode>> Map)
    {
        return new PathSearchNode(this, Map);
    }
}
