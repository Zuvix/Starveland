using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PathSearchNode
{
    public MapCell WrappedCell {get;}
    public PathSearchNode Previous { get; set; }
    public List<List<PathSearchNode>> Map { get; set; }

    public int gCost { get; private set; }
    public int hCost { get; private set; }
    public int fCost { get; private set; }

    public PathSearchNode(MapCell WrappedCell, List<List<PathSearchNode>> Map)
    {
        this.WrappedCell = WrappedCell;
        this.Previous = null;
        this.hCost = 0;
        this.UpdategCost(int.MaxValue);
        this.Map = Map;
    }

    public void UpdategCost(int gCost)
    {
        this.gCost = gCost;
        this.fCost = this.gCost + this.hCost;
    }
    public void UpdatehCost(int hCost)
    {
        this.hCost = hCost;
        this.fCost = this.gCost + this.hCost;
    }
}
