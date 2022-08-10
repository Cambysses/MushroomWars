using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEditor;
using System.Diagnostics;

public class Testing : MonoBehaviour
{
    private PlayerControls controls;
    private MovementArrow movementArrow;
    private Pathfinding pathfinding;
    private GameObject marcel;
    private GameObject cursor;
    private PathNode activeArrowNode;
    private List<Vector3> goodPositions;
    private int movement = 1;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        pathfinding = new Pathfinding(50, 50);
        movementArrow = new MovementArrow();
        goodPositions = new List<Vector3>();
        controls.Player.LeftClick.performed += ctx => ClickGridObject(ctx);
        controls.Player.RightClick.performed += ctx => ShowMovementArea(ctx);
        marcel = GameObject.Find("Marcel");
        cursor = GameObject.Find("Cursor");
    }

    void Update()
    {
        if (!marcel.GetComponent<PlayerMovement>().isMoving)
        {
            MouseOverTile();
        }
    }

    private void ShowMovementArea(InputAction.CallbackContext ctx)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        pathfinding.GetGrid().GetXYFromPosition(marcel.transform.position, out int marcelX, out int marcelY);
        List<Vector3> moveableTiles = pathfinding.FindMovementArea(movement, pathfinding.GetNode(marcelX-1, marcelY-1));
        //List<Vector3> moveableTiles = pathfinding.FindMovementArea(movement, marcel.transform.position);
        foreach (Vector3 tile in moveableTiles)
        {
            DrawMovementTile(tile + new Vector3(1, 1));
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log($"#{movement}: " + stopwatch.ElapsedMilliseconds + "ms");
        stopwatch.Reset();
        movement++;
    }

    private void DrawMovementTile(Vector3 position)
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/UI/movement_tile");
        GameObject spriteObject = new GameObject() { name = "MovementTile", tag = "MovementTile" };
        SpriteRenderer sr = spriteObject.AddComponent<SpriteRenderer>();
        spriteObject.transform.position = position;
        sr.sprite = sprite;
    }

    private void DeleteMovementTiles()
    {
        GameObject[] movementTiles = GameObject.FindGameObjectsWithTag("MovementTile");
        foreach (GameObject movementTile in movementTiles)
        {
            Destroy(movementTile);
        }
    }

    private void MouseOverTile()
    {
        PathNode newArrowNode = GetMouseNode();
        if (newArrowNode == null || activeArrowNode == newArrowNode)
        {
            return;
        }

        activeArrowNode = newArrowNode;
        Vector3 nodePosition = pathfinding.GetGrid().GetWorldPosition(activeArrowNode.x, activeArrowNode.y);
        List<Vector3> vectorPath = pathfinding.FindPath(marcel.transform.position, nodePosition);

        // Draw map cursor
        bool cursorToTheLeftOfPlayer = marcel.transform.position.x > nodePosition.x;
        DrawMapCursor(nodePosition, cursorToTheLeftOfPlayer);

        // Draw Arrow
        if (vectorPath != null)
        {
            movementArrow.EraseArrows();
            movementArrow.DrawArrowPath(vectorPath);
        }
    }

    private void ClickGridObject(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PathNode clickedNode = GetMouseNode();
            pathfinding.GetGrid().GetXYFromPosition(marcel.transform.position, out int marcelX, out int marcelY);
            List<PathNode> path = pathfinding.FindPath(marcelX - 1, marcelY - 1, clickedNode.x, clickedNode.y);
            List<Vector3> vectorPath = pathfinding.FindPath(marcel.transform.position, pathfinding.GetGrid().GetWorldPosition(clickedNode.x, clickedNode.y));
            if (vectorPath != null)
            {
                marcel.GetComponent<PlayerMovement>().SetMoveList(pathfinding.FindPath(marcel.transform.position, pathfinding.GetGrid().GetWorldPosition(clickedNode.x, clickedNode.y)));
            }
        }
    }

    private PathNode GetMouseNode()
    {
        Vector3 mousePosition = GetMousePosition();
        pathfinding.GetGrid().GetXYFromPosition(mousePosition, out int x, out int y);
        return pathfinding.GetNode(x, y);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = (Vector3)Mouse.current.position.ReadValue();
        mousePosition.z = 0;
        return Camera.main.ScreenToWorldPoint(mousePosition) - new Vector3(pathfinding.GetGrid().offset, pathfinding.GetGrid().offset);
    }

    private void DrawMapCursor(Vector3 nodePosition, bool flip)
    {
        int xOffset = flip ? 18 : -2;
        int yOffset = flip ? 5 : 5;

        cursor.transform.position = new Vector3(nodePosition.x, nodePosition.y - 1) + new Vector3(MathUtilities.PixelsToUnits(xOffset), MathUtilities.PixelsToUnits(yOffset));
        cursor.GetComponent<SpriteRenderer>().flipX = flip;
    }
}
