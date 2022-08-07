using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    public static Pathfinding Instance { get; private set; }

    // Heuristic cost of a regular move
    private const int STRAIGHT = 10;

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(int width, int height)
    {
        Instance = this;
        grid = new Grid<PathNode>(width, height, 1f, new Vector3(-10, -9), (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

    private bool CheckWalkable(PathNode node)
    {
        groundTilemap = GameObject.Find("Walkable").GetComponent<Tilemap>();
        collisionTilemap = GameObject.Find("Collidable").GetComponent<Tilemap>();
        Vector3 worldPosition = grid.GetWorldPosition(node.x, node.y);
        Vector3Int gridPosition = groundTilemap.WorldToCell(new Vector3(worldPosition.x, worldPosition.y));
        bool walkable = groundTilemap.HasTile(gridPosition) && !collisionTilemap.HasTile(gridPosition) ? true : false;
        return walkable;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        grid.GetXYFromPosition(startWorldPosition, out int startX, out int startY);
        grid.GetXYFromPosition(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX - 1, startY - 1, endX, endY);

        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path)
            {
                vectorPath.Add(grid.GetWorldPosition(pathNode.x, pathNode.y) + new Vector3(0.5f, 0.5f));
            }
            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int StartY, int EndX, int EndY)
    {
        PathNode startNode = grid.GetGridObject(startX, StartY);
        PathNode endNode = grid.GetGridObject(EndX, EndY);
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // Reached final node.
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                // Node is already in closed list.
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                // Node is not walkable.
                neighbourNode.isWalkable = CheckWalkable(neighbourNode);
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on openList. No usable path.
        Debug.Log("No path found.");
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
        }

        if (currentNode.x + 1 < grid.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
        }

        if (currentNode.y - 1 >= 0)
        {
            // Up
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }

        if (currentNode.y + 1 < grid.GetHeight())
        {
            // Down
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighbourList;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return STRAIGHT + Mathf.Min(xDistance, yDistance) * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];

        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }

}