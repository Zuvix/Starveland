using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class PathFinding : Singleton<PathFinding>
{
    public static readonly bool EXCLUDE_LAST = true;

    public (List<MapCell>, MapCell) FindPath(MapCell Start, List<MapCell> Finishes, bool ExcludeLast = false)
    {

        // Let's Find Path to the closest Storage
        List<MapCell> Path = null;
        List<MapCell> PathTemp = null;
        MapCell ClosestFinish = null;
        foreach (MapCell Finish in Finishes)
        {
            PathTemp = PathFinding.Instance.FindPath(Start, Finish, ExcludeLast);
            if (Path == null)
            {
                ClosestFinish = Finish;
                Path = PathTemp;
            }
            else if (PathTemp != null)
            {
                if (PathTemp.Count < Path.Count)
                {
                    ClosestFinish = Finish;
                    Path = PathTemp;
                }
            }
        }

        return (Path, ClosestFinish);
    }
    public List<MapCell> FindPath(MapCell Start, MapCell Finish, bool ExcludeLast = false)
    {
        // Best be prepared for this (is likely to happen)
        if (Start == null || Finish == null)
        {
            return null;
        }

        List<MapCell> Path = new List<MapCell>();
        // Best be prepared that pathfinding may be trivial
        if (Finish == Start)
        {
            return Path;
        }

        PathSearchStartFinishWrapper StartFinishWrapper = new PathSearchStartFinishWrapper(Start, Finish);
        List<List<PathSearchNode>> SearchedMatrix = MapControl.Instance.map.ProducePathSearchMatrix(StartFinishWrapper);
        List<PathSearchNode> List2BeSearched = new List<PathSearchNode>(new PathSearchNode[] { StartFinishWrapper.StartWrapper });
        List<PathSearchNode> ListAlreadySearched = new List<PathSearchNode>();

        StartFinishWrapper.StartWrapper.UpdategCost(0);
        StartFinishWrapper.StartWrapper.UpdatehCost(PathLength(Start, Finish));

        bool FinishWasFound = false;

        while (List2BeSearched.Count() > 0)
        {
            PathSearchNode CurrentNode = LowestFValueNode(List2BeSearched);
            List2BeSearched.Remove(CurrentNode);
            ListAlreadySearched.Add(CurrentNode);

            if (CurrentNode == StartFinishWrapper.FinishWrapper)
            {
                FinishWasFound = true;
                break;
            }

            foreach (PathSearchNode Neighbour in Neighbours(CurrentNode))
            {
                if (ListAlreadySearched.Contains(Neighbour))
                {
                    continue;
                }
                if (!Neighbour.WrappedCell.CanBeEnteredByUnit() && Neighbour != StartFinishWrapper.FinishWrapper)
                {
                    ListAlreadySearched.Add(Neighbour);
                    continue;
                }

                int tentativeGCost = CurrentNode.gCost + PathLength(CurrentNode, Neighbour);
                if (tentativeGCost < Neighbour.gCost)
                {
                    Neighbour.Previous = CurrentNode;
                    Neighbour.UpdategCost(tentativeGCost);
                    Neighbour.UpdatehCost(PathLength(CurrentNode.WrappedCell, Finish));

                    if (!List2BeSearched.Contains(Neighbour))
                    {
                        List2BeSearched.Add(Neighbour);
                    }
                }
            }
        }

        if (FinishWasFound)
        {
            ReconstructPath(Path, StartFinishWrapper.FinishWrapper);
            if (ExcludeLast && Path.Count > 0)
            {
                Path.RemoveAt(Path.Count - 1);
            }
        }
        else
        {
            Path = null;
        }

        return Path;
    }
    private int PathLength(PathSearchNode Start, PathSearchNode Finish)
    {
        return PathLength(Start.WrappedCell, Finish.WrappedCell);
    }
    private int PathLength(MapCell Start, MapCell Finish)
    {
        //TODO This has to consider that two nodes are not in the same map
        return Math.Abs(Start.x - Finish.x) + Math.Abs(Start.y - Finish.y);
    }
    private PathSearchNode LowestFValueNode(List<PathSearchNode> NodeList) {
        PathSearchNode LowestFValueNode = NodeList[0];
        for (int i = 1; i < NodeList.Count; i++)
        {
            if (NodeList[i].fCost < LowestFValueNode.fCost)
            {
                LowestFValueNode = NodeList[i];
            }
        }
        return LowestFValueNode;
    }

    private List<PathSearchNode> Neighbours(PathSearchNode SearchedNode)
    {
        // Take matrix from node attribute
        List<PathSearchNode> NeighbourList = new List<PathSearchNode>();

        if (SearchedNode.WrappedCell.x > 0)
        {
            NeighbourList.Add(SearchedNode.Map[SearchedNode.WrappedCell.x - 1][SearchedNode.WrappedCell.y]);
        }
        if (SearchedNode.WrappedCell.x < SearchedNode.Map.Count - 1)
        {
            NeighbourList.Add(SearchedNode.Map[SearchedNode.WrappedCell.x + 1][SearchedNode.WrappedCell.y]);
        }
        if (SearchedNode.WrappedCell.y > 0)
        {
            NeighbourList.Add(SearchedNode.Map[SearchedNode.WrappedCell.x][SearchedNode.WrappedCell.y - 1]);
        }
        if (SearchedNode.WrappedCell.y < SearchedNode.Map[0].Count - 1)
        {
            NeighbourList.Add(SearchedNode.Map[SearchedNode.WrappedCell.x][SearchedNode.WrappedCell.y + 1]);
        }
        if (SearchedNode is PathSearchNodeComposite)
        {
            NeighbourList.Add(((PathSearchNodeComposite)SearchedNode).EntryPoint);
        }

        return NeighbourList;
    }
    private void ReconstructPath(List<MapCell> Path, PathSearchNode FinishNode)
    {
        PathSearchNode CurrentNode = FinishNode;
        while (CurrentNode.Previous != null)
        {
            Path.Add(CurrentNode.WrappedCell);
            CurrentNode = CurrentNode.Previous;
        }
        Path.Reverse();
    }
    public int BlockDistance(MapCell A, MapCell B)
    {
        return Math.Abs(A.x - B.x) + Math.Abs(A.y - B.y);
    }
}