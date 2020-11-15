using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PathSearchNodeComposite : PathSearchNode
{
    public List<List<PathSearchNode>> InnerMap { get; set; } = null;
    public PathSearchNodeComposite EntryPoint { get; set; } = null;
    public PathSearchNodeComposite(MapCellComposite WrappedCell, List<List<PathSearchNode>> Map) : base(WrappedCell, Map)
    {
    }
}
