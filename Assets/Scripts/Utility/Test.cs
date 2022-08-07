using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEditor;

public class Test : MonoBehaviour
{
    public PlayerControls controls;
    private Pathfinding pathfinding;
    private GameObject marcel;
    private PathNode activeArrowNode;

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
        pathfinding = new Pathfinding(31, 32);
        controls.Player.LeftClick.performed += ctx => ClickGridObject(ctx);
        marcel = GameObject.Find("Marcel");
    }

    private void Update()
    {
        MouseOverTile();
    }

    private void MouseOverTile()
    {
        PathNode newArrowNode = GetMouseNode();
        if (newArrowNode == null || activeArrowNode == newArrowNode)
        {
            return;
        }

        activeArrowNode = newArrowNode;
        Vector3 position = pathfinding.GetGrid().GetWorldPosition(activeArrowNode.x, activeArrowNode.y);
        List<Vector3> vectorPath = pathfinding.FindPath(marcel.transform.position, position);
        if (vectorPath != null)
        {
            MovementArrow.EraseArrows();
            MovementArrow.DrawArrowPath(vectorPath);
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

            //DrawPath(path);
            // marcel.GetComponent<PlayerMovement>().SetMoveList(pathfinding.FindPath(marcel.transform.position, pathfinding.GetGrid().GetWorldPosition(clickedNode.x, clickedNode.y)));
        }
    }

    public PathNode GetMouseNode()
    {
        Vector3 mousePosition = GetMousePosition();
        pathfinding.GetGrid().GetXYFromPosition(mousePosition, out int x, out int y);
        return pathfinding.GetNode(x, y);
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePosition = (Vector3)Mouse.current.position.ReadValue();
        mousePosition.z = 0;
        return Camera.main.ScreenToWorldPoint(mousePosition) - new Vector3(pathfinding.GetGrid().offset, pathfinding.GetGrid().offset);
    }

}
