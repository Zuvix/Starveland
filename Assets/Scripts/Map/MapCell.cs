using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private CellObject ObjectBackup;
    private static System.Random Random = null;
    public MapCell(Vector3 position, Map Map, int x, int y)
    {
        this.position = position;
        this.Map = Map;
        this.x = x;
        this.y = y;

        if (Random == null)
        {
            Random = new System.Random();
        }
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

            if(CurrentObject != null)
            {
                this.CurrentObject.MakeTransparent(0.25f);
            }
        }
        return Success;
    }
    public void EraseCellObject()
    {
        if (this.CurrentObject != null)
        {
            GameObject ReplacementObject = null;
            if (this.CurrentObject.Replacement != null)
            {
                ReplacementObject = this.CurrentObject.Replacement;
            }
            CellObject.Destroy(this.CurrentObject.gameObject);
            this.CurrentObject = null;
            if (ReplacementObject != null)
            {
                MapControl.Instance.CreateGameObject(this.x, this.y, ReplacementObject);
            }
        }
    }
    public void EraseUnit()
    {
        this.CurrentUnit = null;
        if (this.CurrentObject != null)
        {
            this.CurrentObject.MakeOpaque();
        }
    }
    public bool ReplaceCellObject(CellObject CellObject)
    {
        if (this.CurrentObject != null)
        {
            CellObject.Destroy(this.CurrentObject.gameObject);
        }
        this.CurrentObject = null;
        return SetCellObject(CellObject);
    }
    public bool CanBeEnteredByObject(bool EnteringObjectIsBlocking)
    {
        return (this.CurrentObject == null && (this.CurrentUnit == null || !EnteringObjectIsBlocking)) || (this.CurrentObject!=null && !this.CurrentObject.IsBlocking && EnteringObjectIsBlocking && this.CurrentUnit==null);
    }
    public bool CanBeEnteredByUnit()
    {
        return this.CurrentUnit == null && (this.CurrentObject == null || !this.CurrentObject.IsBlocking);
    }
    public void BackupCellObject()
    {
        if (this.CurrentObject != null)
        {
            this.ObjectBackup = this.CurrentObject;
            this.ObjectBackup.gameObject.SetActive(false);
            this.CurrentObject = null;
        }
    }
    public void RestoreCellObject()
    {
        if (this.ObjectBackup != null)
        {
            this.CurrentObject = this.ObjectBackup;
            this.CurrentObject.gameObject.SetActive(true);
            this.ObjectBackup = null;
        }
    }
    public void DiscardCellObjectBackup()
    {
        if (this.ObjectBackup != null)
        {
            CellObject.Destroy(this.ObjectBackup);
            this.ObjectBackup = null;
        }
    }
    public virtual PathSearchNode ProducePathSearchNode(List<List<PathSearchNode>> Map)
    {
        return new PathSearchNode(this, Map);
    }

    public List<MapCell> GetClosestNeighbours()
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
    public List<MapCell> GetClosestDiagonalNeighbours()
    {
        List<MapCell> Neighbours = new List<MapCell>();
        if (this.x > 0 && this.y > 0)
        {
            Neighbours.Add(this.Map.Grid[this.x - 1][this.y - 1]);
        }
        if (this.x < this.Map.Grid.Count - 1 && this.y > 0)
        {
            Neighbours.Add(this.Map.Grid[this.x + 1][this.y - 1]);
        }
        if (this.x > 0 && this.y < this.Map.Grid[0].Count - 1)
        {
            Neighbours.Add(this.Map.Grid[this.x - 1][this.y + 1]);
        }
        if (this.x < this.Map.Grid.Count - 1 && this.y < this.Map.Grid[0].Count - 1)
        {
            Neighbours.Add(this.Map.Grid[this.x + 1][this.y + 1]);
        }

        return Neighbours;
    }
    public MapCell GetRandomUnitEnterableNeighbour()
    {
        MapCell Result = null;
        List<MapCell> Possibilities = GetClosestNeighbours().Where(x => x.CanBeEnteredByUnit()).ToList();
        if (Possibilities.Count <= 0)
        {
            Possibilities = GetClosestDiagonalNeighbours().Where(x => x.CanBeEnteredByUnit()).ToList();
            if (Possibilities.Count <= 0)
            {
                int UpperBound = new int[] { x, y, Map.Grid.Count, Map.Grid[0].Count}.Max();
                for (int i = 2; i <= UpperBound; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        List<(int, int)> PotentialCoordinates = new List<(int, int)>(new (int, int)[] {
                            (x + i, y + j),
                            (x - i, y + j),
                            (x + i, y - j),
                            (x - i, y - j),
                            (x + j, y + i),
                            (x - j, y + i),
                            (x + j, y - i),
                            (x - j, y - i)
                        });
                        foreach ((int, int) Coordinates in PotentialCoordinates)
                        {
                            if (Map.IsInBounds(Coordinates.Item1, Coordinates.Item2) && Map.Grid[Coordinates.Item1][Coordinates.Item2].CanBeEnteredByUnit())
                            {
                                Result = Map.Grid[Coordinates.Item1][Coordinates.Item2];
                                break;
                            }
                        }
                        if (Result != null)
                        {
                            break;
                        }
                    }
                    if (Result != null)
                    {
                        break;
                    }
                }
            }
        }
        if (Result == null && Possibilities.Count > 0)
        {
            Result = Possibilities[Random.Next(0, Possibilities.Count - 1)];
        }
        return Result;
    }

    public List<MapCell> GetRandomNeighbouringResourceSourceSpawnLocation(int resourceSourcesToSpawn)
    {
        List<MapCell> Possibilities = GetClosestNeighbours().Where(x => x.CanBeEnteredByObject(true)).ToList();
        Possibilities.Union(GetClosestDiagonalNeighbours().Where(x => x.CanBeEnteredByObject(true)).ToList());

        if (Possibilities.Count <= resourceSourcesToSpawn)
        {
            int UpperBound = new int[] { x, y, Map.Grid.Count, Map.Grid[0].Count }.Max();
            for (int i = 2; i <= UpperBound; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    List<(int, int)> PotentialCoordinates = new List<(int, int)>(new (int, int)[] {
                        (x + i, y + j),
                        (x - i, y + j),
                        (x + i, y - j),
                        (x - i, y - j),
                        (x + j, y + i),
                        (x - j, y + i),
                        (x + j, y - i),
                        (x - j, y - i)
                    });
                    foreach ((int, int) Coordinates in PotentialCoordinates)
                    {
                        if (Map.IsInBounds(Coordinates.Item1, Coordinates.Item2) && Map.Grid[Coordinates.Item1][Coordinates.Item2].CanBeEnteredByObject(true))
                        {
                            Possibilities.Add(Map.Grid[Coordinates.Item1][Coordinates.Item2]);
                            break;
                        }
                    }
                    if (Possibilities.Count >= resourceSourcesToSpawn)
                    {
                        break;
                    }
                }
                if (Possibilities.Count >= resourceSourcesToSpawn)
                {
                    break;
                }
            }
        }       
        if (Possibilities.Count > resourceSourcesToSpawn)
        {
            while (Possibilities.Count > resourceSourcesToSpawn)
            {
                Possibilities.RemoveAt(UnityEngine.Random.Range(0, Possibilities.Count - 1));
            }
        }
        return Possibilities;
    }
}
