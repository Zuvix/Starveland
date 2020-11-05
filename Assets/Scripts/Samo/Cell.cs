using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3 position;
    GameObject cellGameObject = null;
    Map inMap;
    public Cell(Vector3 position, GameObject g)
    {
        this.position = position;
        this.cellGameObject = g;
    }
    public void SetCellObject(GameObject g)
    {
        cellGameObject = g;
    }
    public GameObject GetCellObject()
    {
        return cellGameObject;
    }

}
