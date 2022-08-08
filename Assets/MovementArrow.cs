using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementArrow : MonoBehaviour
{

    public Sprite arrowTailDown;
    public Sprite arrowTailUp;
    public Sprite arrowTailLeft;
    public Sprite arrowTailRight;
    public Sprite arrowDown;
    public Sprite arrowUp;
    public Sprite arrowLeft;
    public Sprite arrowRight;
    public Sprite arrowStraightHorizontal;
    public Sprite arrowStraightVertical;
    public Sprite arrowCurveBottomLeft;
    public Sprite arrowCurveBottomRight;
    public Sprite arrowCurveTopLeft;
    public Sprite arrowCurveTopRight;


    public void DrawArrowPath(List<Vector3> path)
    {

        for (int i = 0; i < path.Count; i++)
        {
            if (path.Count == 1)
            {
                return;
            }

            Vector3 cur = path[i];
            Vector3 next = i < path.Count-1 ? path[i+1] : Vector3.zero;
            Vector3 prev = i > 0 ? path[i-1] : Vector3.zero;

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


    private static void DrawSegment(Vector3 position, Sprite sprite)
    {
        GameObject spriteObject = new GameObject() { tag = "MovementArrow" };
        spriteObject.AddComponent<SpriteRenderer>();

        spriteObject.transform.position = position;
        spriteObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void EraseArrows()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("MovementArrow");
        foreach (GameObject arrow in arrows)
        {
            Destroy(arrow);
        }
    }

}
