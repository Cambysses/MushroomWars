using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathNode : IHeapItem<PathNode>
{
    private Grid<PathNode> grid;

    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int fCost;
    public bool isWalkable;
    public PathNode cameFromNode;
    public Vector3 worldPosition;
    public Vector3Int gridPosition;
    public List<PathNode> neighbours;
    private int heapIndex;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public override string ToString()
    {
        return x + "," + y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(PathNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
