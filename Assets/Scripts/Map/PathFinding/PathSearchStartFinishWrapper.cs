using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PathSearchStartFinishWrapper
{
    private MapCell Start;
    private MapCell Finish;
    public PathSearchNode StartWrapper;
    public PathSearchNode FinishWrapper;
    public PathSearchStartFinishWrapper(MapCell Start, MapCell Finish)
    {
        this.Start = Start;
        this.Finish = Finish;
        this.StartWrapper = null;
        this.FinishWrapper = null;
    }
    public void TryAddNode(PathSearchNode Candidate)
    {
        if (Candidate.WrappedCell == Start)
        {
            this.StartWrapper = Candidate;
        }
        else if (Candidate.WrappedCell == Finish)
        {
            this.FinishWrapper = Candidate;
        }
    }
}
