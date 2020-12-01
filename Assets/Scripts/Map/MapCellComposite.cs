using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MapCellComposite : MapCell
{
    public Map InnerMap { get; }
    public MapCellComposite EntryPoint { get; private set; }

    public MapCellComposite(Vector3 position, Map Map, int x, int y, Map InnerMap) : base(position, Map, x, y)
    {
        this.InnerMap = InnerMap;
        this.EntryPoint = null;
    }

    public void SetEntryPoint(MapCellComposite EntryPoint)
    {
        this.EntryPoint = EntryPoint;
        EntryPoint.EntryPoint = this;
    }
}
