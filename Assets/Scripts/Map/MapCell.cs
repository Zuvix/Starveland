using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell
{
    public static bool BLOCKING = true;
    public static bool NONBLOCKING = false;
    public int x { get; }
    public int y { get; }
    private CellObject CurrentBlockingObject { get; set; } = null;
    private CellObject CurrentNonblockingObject { get; set; } = null;
    public Map Map { get; }

    public Vector3 position;
    public MapCell(Vector3 position, GameObject g, Map Map, int x, int y)
    {
        this.position = position;
        this.SetCellObject(g);
        this.Map = Map;
        this.x = x;
        this.y = y;
    }
    public void SetCellObject(GameObject g)
    {
        if (g != null)
        {
            g.GetComponent<CellObject>().SetCurrentCell(this);
            this.AddCellObjectToCell(g.GetComponent<CellObject>());
        }
    }
    public void AddCellObjectToCell(CellObject CellObject)
    {
        if (CellObject.IsBlocking)
        {
            this.CurrentBlockingObject = CellObject;
        }
        else
        {
            this.CurrentNonblockingObject = CellObject;
        }
    }
    public void EraseCellObjectBlocking()
    {
        this.CurrentBlockingObject = null;
    }
    public void EraseCellObjectNonblocking()
    {
        this.CurrentNonblockingObject = null;
    }
    public void EraseCellObject(CellObject CellObject)
    {
        if (CellObject == null)
        {
            return;
        }

        if (this.CurrentBlockingObject == CellObject)
        {
            this.CurrentBlockingObject = null;
        }
        else if (this.CurrentNonblockingObject == CellObject)
        {
            this.CurrentNonblockingObject = null;
        }
        else
        {
            throw new Exception("Tried to remove cellobject from mapcell when that cellobject wasn't there");
        }
    }
    public CellObject GetTopSelectableObject()
    {
        CellObject Result = null;
        if (this.CurrentBlockingObject != null && this.CurrentBlockingObject.IsSelectable)
        {
            Result = CurrentBlockingObject;
        }
        else if (this.CurrentNonblockingObject != null && this.CurrentNonblockingObject.IsSelectable)
        {
            Result = CurrentNonblockingObject;
        }
        return Result;
    }
    public CellObject GetCurrentCellObject(bool Blocking)
    {
        CellObject Result = null;
        if (Blocking)
        {
            Result = this.CurrentBlockingObject;
        }
        else
        {
            Result = this.CurrentNonblockingObject;
        }
        return Result;
    }
    public ResourceSource GetCurrentResourceSource()
    {
        ResourceSource Result = null;
        if (this.CurrentBlockingObject is ResourceSource)
        {
            Result = (ResourceSource)this.CurrentBlockingObject;
        }
        else if (this.CurrentNonblockingObject is ResourceSource)
        {
            Result = (ResourceSource)this.CurrentNonblockingObject;
        }
        return Result;
    }
    public bool CanBeEntered()
    {
        return this.CurrentBlockingObject == null;
    }
    public virtual PathSearchNode ProducePathSearchNode(List<List<PathSearchNode>> Map)
    {
        return new PathSearchNode(this, Map);
    }
    public MapCell GetRandomNeighbour()
    {
        List<MapCell> Neighbours = GetNeighbours();
        MapCell Result = null;
        if (Neighbours.Count > 0)
        {
            Result = Neighbours[UnityEngine.Random.Range(0, Neighbours.Count)];
        }
        return Result;
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
