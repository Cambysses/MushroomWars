using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementArrow
{
    private Sprite[] sprites;
    private Sprite arrowTailDown;
    private Sprite arrowTailUp;
    private Sprite arrowTailLeft;
    private Sprite arrowTailRight;
    private Sprite arrowDown;
    private Sprite arrowUp;
    private Sprite arrowLeft;
    private Sprite arrowRight;
    private Sprite arrowStraightHorizontal;
    private Sprite arrowStraightVertical;
    private Sprite arrowCurveBottomLeft;
    private Sprite arrowCurveBottomRight;
    private Sprite arrowCurveTopLeft;
    private Sprite arrowCurveTopRight;

    public MovementArrow()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/UI/movement_arrows");
        this.arrowTailDown = sprites.Where(s => s.name == "arrow_tail_down").First();
        this.arrowTailUp = sprites.Where(s => s.name == "arrow_tail_up").First();
        this.arrowTailLeft = sprites.Where(s => s.name == "arrow_tail_left").First();
        this.arrowTailRight = sprites.Where(s => s.name == "arrow_tail_right").First();
        this.arrowDown = sprites.Where(s => s.name == "arrow_down").First();
        this.arrowUp = sprites.Where(s => s.name == "arrow_up").First();
        this.arrowLeft = sprites.Where(s => s.name == "arrow_left").First();
        this.arrowRight = sprites.Where(s => s.name == "arrow_right").First();
        this.arrowStraightHorizontal = sprites.Where(s => s.name == "arrow_straight_horizontal").First();
        this.arrowStraightVertical = sprites.Where(s => s.name == "arrow_straight_vertical").First();
        this.arrowCurveBottomLeft = sprites.Where(s => s.name == "arrow_curve_bottomleft").First();
        this.arrowCurveBottomRight = sprites.Where(s => s.name == "arrow_curve_bottomright").First();
        this.arrowCurveTopLeft = sprites.Where(s => s.name == "arrow_curve_topleft").First();
        this.arrowCurveTopRight = sprites.Where(s => s.name == "arrow_curve_topright").First();
    }

    public void DrawArrowPath(List<Vector3> path)
    {

        for (int i = 0; i < path.Count; i++)
        {
            if (path.Count == 1)
            {
                return;
            }

            Vector3 cur = path[i];
            Vector3 next = i < path.Count - 1 ? path[i + 1] : Vector3.zero;
            Vector3 prev = i > 0 ? path[i - 1] : Vector3.zero;

            bool tailDown = next.y > cur.y;
            bool tailUp = next.y < cur.y;
            bool tailLeft = next.x > cur.x;
            bool tailRight = next.x < cur.x;
            bool headDown = prev.y > cur.y;
            bool headUp = prev.y < cur.y;
            bool headLeft = prev.x > cur.x;
            bool headRight = prev.x < cur.x;

            // Draw tail
            if (i == 0)
            {
                // Arrow moving up
                if (tailDown)
                {
                    DrawSegment(cur, arrowTailDown);
                }

                // Arrow moving down
                else if (tailUp)
                {
                    DrawSegment(cur, arrowTailUp);
                }

                // Arrow moving right
                else if (tailLeft)
                {
                    DrawSegment(cur, arrowTailLeft);
                }

                // Arrow moving left
                else if (tailRight)
                {
                    DrawSegment(cur, arrowTailRight);
                }
            }
            // Draw head
            else if (i == path.Count - 1)
            {
                // Arrow moving up
                if (headDown)
                {
                    DrawSegment(cur, arrowDown);
                }

                // Arrow moving down
                else if (headUp)
                {
                    DrawSegment(cur, arrowUp);
                }

                // Arrow moving right
                else if (headLeft)
                {
                    DrawSegment(cur, arrowLeft);
                }

                // Arrow moving left
                else
                {
                    DrawSegment(cur, arrowRight);
                }
            }
            // Draw straight line
            else if (path.Count > 2 && ((cur.y == prev.y && cur.y == next.y) || (cur.x == prev.x && cur.x == next.x)))
            {
                if (cur.y == prev.y)
                {
                    DrawSegment(cur, arrowStraightHorizontal);
                }
                else
                {
                    DrawSegment(cur, arrowStraightVertical);
                }
            }
            // Draw curve
            else if ((headRight && tailDown) || (headDown && tailRight))
            {
                DrawSegment(cur, arrowCurveBottomRight);
            }
            // Bottom left curve
            else if ((headLeft && tailDown) || (headDown && tailLeft))
            {
                DrawSegment(cur, arrowCurveBottomLeft);
            }
            // Top left curve
            else if ((headLeft && tailUp) || (tailLeft && headUp))
            {
                DrawSegment(cur, arrowCurveTopLeft);
            }
            // Top right curve
            else if ((headRight && tailUp) || (tailRight && headUp))
            {
                DrawSegment(cur, arrowCurveTopRight);
            }
        }
    }


    private void DrawSegment(Vector3 position, Sprite sprite)
    {
        GameObject spriteObject = new GameObject() { name = "MovementArrow", tag = "MovementArrow" };
        spriteObject.AddComponent<SpriteRenderer>();

        spriteObject.transform.position = position;
        spriteObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void EraseArrows()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("MovementArrow");
        foreach (GameObject arrow in arrows)
        {
            UnityEngine.MonoBehaviour.Destroy(arrow);
        }
    }

}
