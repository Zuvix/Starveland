using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sapling : MonoBehaviour
{
    public int DaysToGrow = 1;
    private CellObject co;

    private void Awake()
    {
        DaytimeCounter.Instance.OnDayOver.AddListener(Grow);
    }

    private void Start()
    {
        co = GetComponent<ResourceSource>();
    }

    private void Grow()
    {
        this.DaysToGrow--;
        if (DaysToGrow <= 0)
        {
            int x = co.CurrentCell.x;
            int y = co.CurrentCell.y;
            co.CurrentCell.EraseCellObject();
            ResourceSourceFactory.Instance.ProduceResourceSource(x, y, RSObjects.Forest);
        }
    }
}
