using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Map {

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    public List<List<Cell>> cells;

    public Map(int width, int height, float cellSize, Vector3 originPosition) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        InitializeCellArray(width, height, true);
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

    public float GetCellSize() {
        return cellSize;
    }

    public void InitializeCellArray(int width, int height,bool debugLines)
    {
        cells = new List<List<Cell>>();
        for (int x = 0; x < width; x++)
        {
            cells.Add(new List<Cell>());
            for (int y = 0; y < height; y++)
            {
                cells[x].Add(new Cell(GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, null));
                if (debugLines)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            if (debugLines)
            {
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            }
        }
    }
    //Vstup su indexy x a y prvku v gride, vysledok je pozicia vo svete
    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, GameObject o) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            cells[x][y].SetCellObject(o);
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    public void SetValue(Vector3 worldPosition, GameObject o) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, o);
    }

    public GameObject GetValue(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return cells[x][y].GetCellObject();
        } else {
            return null;
        }
    }

    public GameObject GetValue(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public void CenterObject(int x, int y,GameObject g)
    {
        g.transform.position = new Vector3 (cells[x][y].position.x, cells[x][y].position.y,0);
    }
    public bool IsInBounds(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return IsInBounds(x, y);
    }
    public bool IsInBounds(int x,int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return true;
        }
        Debug.Log("Target outside of bounds.");
        return false;
    }
}
