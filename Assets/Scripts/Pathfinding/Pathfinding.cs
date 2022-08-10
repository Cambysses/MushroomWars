using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding
{
    public static Pathfinding Instance { get; private set; }
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;

    // Heuristic cost of a regular move
    private const int STRAIGHT = 10;

    private Grid<PathNode> grid;
    private Heap<PathNode> openList;
    private List<Vector3> movePositions;
    private List<Vector3> collisionTiles;

    public Pathfinding(int width, int height)
    {
        Instance = this;
        movePositions = new List<Vector3>();
        groundTilemap = GameObject.Find("Walkable").GetComponent<Tilemap>();
        collisionTilemap = GameObject.Find("Collidable").GetComponent<Tilemap>();

        grid = new Grid<PathNode>(width, height, 1f, new Vector3(-30, -30), (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.worldPosition = grid.GetWorldPosition(x, y);
                pathNode.gridPosition = collisionTilemap.WorldToCell(new Vector3(pathNode.worldPosition.x, pathNode.worldPosition.y));
                pathNode.worldPosition += new Vector3(-0.5f, -0.5f); // >:)
                pathNode.isWalkable = !collisionTilemap.HasTile(pathNode.gridPosition);
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }
        // we need to loop over once more, since each pathNode's isWalkable-property isn't set until the previous loop is done.
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.neighbours = GetNeighbourList(pathNode);
            }
        }
    }

    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
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

    public void FindMovementArea(int radius, PathNode startNode, int currentDepth)
    {
        if (!movePositions.Contains(startNode.worldPosition) && currentDepth <= radius)
        {
            movePositions.Add(startNode.worldPosition);
            foreach (PathNode neighbour in startNode.neighbours) 
            {
                FindMovementArea(radius, neighbour, currentDepth + 1);
            }
        }
    }

    public List<Vector3> FindMovementArea(int radius, PathNode startNode)
    {
        movePositions.Clear();
        FindMovementArea(radius, startNode, 0);
        return movePositions;
    }

    public List<Vector3> FindMovementArea(int radius, Vector3 startPosition)
    {
        for (int x = -radius; x <= radius; ++x)
        {
            for (int y = -radius; y <= radius; ++y)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= radius && startPosition != new Vector3(x, y))
                {
                    if (movePositions.Where(mp => mp.x == (startPosition.x + -1 + x) && mp.y == (startPosition.y + -1 + y)).Count() > 0)
                    {
                        continue;
                    }
                    List<Vector3> path = FindPath(startPosition, startPosition + new Vector3(-1, -1) + new Vector3(x, y));
                    if (path != null && radius >= path.Count - 1)
                    {
                        movePositions.Add(path[path.Count - 1] + new Vector3(-1, -1));
                    }
                }
            }
        }

        return movePositions;
    }

    public void ClearPositions()
    {
        movePositions.Clear();
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
        openList = new Heap<PathNode>(grid.GetHeight() * grid.GetWidth());
        openList.Add(startNode);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = openList.RemoveFirst();

            if (currentNode == endNode)
            {
                // Reached final node.
                return CalculatePath(endNode);
            }

            foreach (PathNode neighbourNode in currentNode.neighbours)
            {

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
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        List<int> deltas = new List<int>(){-1, 1};

        foreach (int delta in deltas)
        {
            int newX = currentNode.x + delta;
            int newY = currentNode.y + delta;
            if (newX >= 0 && newX < grid.GetWidth())
            {
                PathNode newNode = GetNode(newX, currentNode.y);
                if (newNode.isWalkable) {
                    neighbourList.Add(newNode);
                }
            }
            if (newY >= 0 && newY < grid.GetHeight())
            {
                PathNode newNode = GetNode(currentNode.x, newY);
                if (newNode.isWalkable) {
                    neighbourList.Add(newNode);
                }
            }
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
}