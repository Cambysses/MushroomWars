using System.Collections.Generic;
using UnityEngine;

public class InterfaceUtilities
{
    private static Pathfinding pathfinding;

    public static void DrawPathLine(List<PathNode> path)
    {
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 worldPoisitionStart = pathfinding.GetGrid().GetWorldPosition(path[i].x, path[i].y);
                Vector3 worldPoisitionEnd = pathfinding.GetGrid().GetWorldPosition(path[i + 1].x, path[i + 1].y);
                Debug.DrawLine(worldPoisitionStart + new Vector3(0.5f, 0.5f), worldPoisitionEnd + new Vector3(0.5f, 0.5f), Color.blue, 1f);
            }
        }
    }
}
