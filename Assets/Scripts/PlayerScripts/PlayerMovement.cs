using System.Collections.Generic;
using UnityEngine;
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
        facing = MathUtilities.GetCardinal(movePosition - transform.position);
        animator.SetInteger("Facing", facing);
        transform.position = Vector3.MoveTowards(transform.position, movePosition, stats.moveSpeed * Time.deltaTime);
    }

    public void SetTileColor(Color color, Vector3Int position, Tilemap tilemap)
    {
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }
}
