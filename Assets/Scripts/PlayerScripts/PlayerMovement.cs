using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    [SerializeField] private Animator animator;
    private PlayerControls controls;

    public CharacterStats stats;
    public GameObject follower;
    public GameObject master;
    public bool isMoving;
    public Vector3 previousPosition;
    public List<Vector3> positionedAlreadyMoved;
    public List<Vector3> movePositions;
    public int facing;

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

    void Start()
    {
        if (stats.isActive)
        {
            follower = GameObject.Find("Liam");
        }
        else
        {
            master = GameObject.Find("Marcel");
        }
    }

    void Update()
    {
        if (movePositions.Count > 0)
        {
            if (transform.position != movePositions[0])
            {
                MoveToPosition(movePositions[0]);
            }
            else
            {
                movePositions.RemoveAt(0);
            }
        }
        else
        {
            foreach (Vector3 position in positionedAlreadyMoved)
            {
                SetTileColor(Color.white, Vector3Int.FloorToInt(position + new Vector3(-1, -1)), GameObject.Find("Walkable").GetComponent<Tilemap>());
            }
            positionedAlreadyMoved.Clear();
            isMoving = false;
        }
    }

    public void SetMoveList(List<Vector3> newMovePositions)
    {
        if (!isMoving)
        {
            isMoving = true;
            positionedAlreadyMoved = new List<Vector3>(newMovePositions);
            PlayerMovement followerMovement = follower.GetComponent<PlayerMovement>();
            List<Vector3> followerMovePositions = new List<Vector3>(newMovePositions);
            followerMovement.movePositions = followerMovePositions;
            followerMovement.movePositions.RemoveAt(followerMovement.movePositions.Count - 1);
            followerMovement.isMoving = true;

            // foreach (Vector3 position in newMovePositions)
            // {
            //     SetTileColor(HexToColor("96D7FF"), Vector3Int.FloorToInt(position + new Vector3(-1, -1)), GameObject.Find("Walkable").GetComponent<Tilemap>());
            // }
            // Vector3 lastPosition = newMovePositions[newMovePositions.Count - 1];
            // SetTileColor(Color.green, Vector3Int.FloorToInt(lastPosition + new Vector3(-1, -1)), GameObject.Find("Walkable").GetComponent<Tilemap>());

            // Remove first node since we are already there.
            if (transform.position == new Vector3(newMovePositions[0].x, newMovePositions[0].y, transform.position.z))
            {
                newMovePositions.RemoveAt(0);
            }

            movePositions = newMovePositions;
        }
    }

    public void MoveToPosition(Vector3 movePosition)
    {
        previousPosition = transform.position;
        facing = GetCardinal(movePosition - transform.position);
        SetAnimation(facing);
        transform.position = Vector3.MoveTowards(transform.position, movePosition, stats.moveSpeed * Time.deltaTime);
    }

    public void SetTileColor(Color color, Vector3Int position, Tilemap tilemap)
    {
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }

    private int GetCardinal(Vector3 direction)
    {
        if (direction.y < 0) { return 0; }
        else if (direction.x > 0) { return 1; }
        else if (direction.y > 0) { return 2; }
        else { return 3; }
    }

    private void SetAnimation(int facing)
    {
        animator.SetInteger("Facing", facing);
    }

    public Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

}
