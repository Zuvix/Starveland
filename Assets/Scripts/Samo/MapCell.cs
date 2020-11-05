using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell
{
    public int x { get; }
    public int y { get; }
    public CellObject CurrentObject { get; set; } = null;
    public Map Map { get; }

    public Vector3 position;
    GameObject cellGameObject = null;
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
        this.cellGameObject = g;
        if (g != null)
        {
            g.GetComponent<CellObject>().SetCurrentCell(this);
            this.CurrentObject = this.cellGameObject.GetComponent<CellObject>();
        }
        else
        {
            this.CurrentObject = null;
        }
    }
    public GameObject GetCellObject()
    {
        return cellGameObject;
    }
    public bool CanBeEntered()
    {
        //TODO
        if (x == 1 && y == 0) return false;
        return true;
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
            Result = Neighbours[Random.Range(0, Neighbours.Count)];
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
