using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{

    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get; set; }

    public bool IsWalkable { get; set; } = true;

    private GridPosition gridPosition;

    public PathNode CameFromPathNode { get; set; }

    public PathNode(GridPosition gridposition)
    {
        this.gridPosition = gridposition;
    }

    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
}
