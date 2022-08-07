using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementArrow : MonoBehaviour
{
    // public static void DrawArrowPath(List<Vector3> path)
    // {
    //     Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/UI");
    //     Sprite sprite = null;

    //     for (int i = 0; i < path.Count; i++)
    //     {
    //         if (path.Count == 1)
    //         {
    //             continue;
    //         }

    //         // Draw tail
    //         if (i == 0)
    //         {
    //             // Arrow moving up
    //             if (path[i + 1].y > path[i].y)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_tail_down").First();
    //             }

    //             // Arrow moving down
    //             else if (path[i + 1].y < path[i].y)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_tail_up").First();
    //             }

    //             // Arrow moving right
    //             else if (path[i + 1].x > path[i].x)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_tail_left").First();
    //             }

    //             // Arrow moving left
    //             else if (path[i + 1].x < path[i].x)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_tail_right").First();
    //             }
    //         }

    //         // Draw head
    //         else if (i == path.Count - 1)
    //         {
    //             // Arrow moving up
    //             if (path[i - 1].y > path[i].y)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_down").First();
    //             }

    //             // Arrow moving down
    //             else if (path[i - 1].y < path[i].y)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_up").First();
    //             }

    //             // Arrow moving right
    //             else if (path[i - 1].x > path[i].x)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_left").First();
    //             }

    //             // Arrow moving left
    //             else
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_right").First();
    //             }
    //         }

    //         // Draw straight line
    //         else if (path.Count > 2 && (path[i].y == path[i - 1].y && path[i].y == path[i + 1].y || path[i].x == path[i - 1].x && path[i].x == path[i + 1].x))
    //         {
    //             if (path[i].y == path[i - 1].y && path[i].y == path[i + 1].y)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_straight_horizontal").First();
    //             }
    //             else
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_straight_vertical").First();
    //             }
    //         }

    //         // Draw curve
    //         else if (path.Count >= 3)
    //         {
    //             // Bottom right curve
    //             if (path[i - 1].x < path[i].x && path[i + 1].y > path[i].y || path[i - 1].y > path[i].y && path[i + 1].x < path[i].x)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_curve_bottomright").First();
    //             }

    //             // Bottom left curve
    //             else if (path[i - 1].x > path[i].x && path[i + 1].y > path[i].y || path[i - 1].y > path[i].y && path[i + 1].x > path[i].x)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_curve_bottomleft").First();
    //             }

    //             // Top left curve
    //             else if (path[i - 1].x > path[i].x && path[i + 1].y < path[i].y || path[i + 1].x > path[i].x && path[i - 1].y < path[i].y)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_curve_topleft").First();
    //             }

    //             // Top right curve
    //             else if (path[i - 1].x < path[i].x && path[i + 1].y < path[i].y || path[i + 1].x < path[i].x && path[i - 1].y < path[i].y)
    //             {
    //                 sprite = sprites.Where(s => s.name == "arrow_curve_topright").First();
    //             }
    //         }

    //         DrawArrow(path[i], sprite);
    //     }
    // }


    // private static void DrawArrow(Vector3 position, Sprite sprite)
    // {
    //     GameObject spriteObject = new GameObject() { tag = "MovementArrow" };
    //     spriteObject.AddComponent<SpriteRenderer>();

    //     spriteObject.transform.position = position;
    //     spriteObject.GetComponent<SpriteRenderer>().sprite = sprite;
    // }



    public static void DrawArrowPath(List<Vector3> path)
    {
        if (path.Count == 1)
        {
            return;
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/UI");

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 prev = i > 1 ? path[i - 1] : Vector3.zero;
            Vector3 cur = path[i];
            Vector3 next = path[i + 1];

            bool tail_down = next.y > cur.y;
            bool tail_up = next.y < cur.y;
            bool tail_left = next.x < cur.x;
            bool tail_right = next.x > cur.x;

            bool head_down = prev.y > cur.y;
            bool head_up = prev.y < cur.y;
            bool head_right = prev.x > cur.x;
            bool head_left = prev.x < cur.x;

            // Draw tail
            if (i == 0)
            {


                if (tail_down)
                {
                    DrawSegment(sprites, cur, "arrow_tail_down");
                }
                else if (tail_up)
                {
                    DrawSegment(sprites, cur, "arrow_tail_up");
                }
                else if (tail_left)
                {
                    DrawSegment(sprites, cur, "arrow_tail_left");
                }
                else
                {
                    DrawSegment(sprites, cur, "arrow_tail_right");
                }
            }

            // Draw head
            else if (i == path.Count - 1)
            {


                // Arrow moving up
                if (head_down)
                {
                    DrawSegment(sprites, cur, "arrow_down");
                }

                // Arrow moving down
                else if (head_up)
                {
                    DrawSegment(sprites, cur, "arrow_up");
                }

                // Arrow moving right
                else if (head_right)
                {
                    DrawSegment(sprites, cur, "arrow_left");
                }

                // Arrow moving left
                else
                {
                    DrawSegment(sprites, cur, "arrow_right");
                }
            }

            // Draw straight line
            else if (path.Count > 2 && (cur.y == prev.y && cur.y == next.y || cur.x == prev.x && cur.x == next.x))
            {
                if (cur.y == prev.y)
                {
                    DrawSegment(sprites, cur, "arrow_straight_horizontal");
                }
                else
                {
                    DrawSegment(sprites, cur, "arrow_straight_vertical");
                }
            }
            // Draw curve
            else if (path.Count >= 3)
            {
                // Bottom right curve
                if (head_left && tail_down || head_down && tail_left)
                {
                    DrawSegment(sprites, cur, "arrow_curve_bottomright");
                }

                // Bottom left curve
                else if (head_right && tail_down || head_down && tail_right)
                {
                    DrawSegment(sprites, cur, "arrow_curve_bottomleft");
                }

                // Top left curve
                else if (head_right && tail_up || tail_right && head_up)
                {
                    DrawSegment(sprites, cur, "arrow_curve_topleft");
                }

                // Top right curve
                else if (head_left && tail_up || tail_left && head_up)
                {
                    DrawSegment(sprites, cur, "arrow_curve_topright");
                }
            }
        }
    }

    private static void DrawSegment(Sprite[] sprites, Vector3 position, string sprite_name)
    {
        GameObject spriteObject = new GameObject() { tag = "MovementArrow" };
        spriteObject.AddComponent<SpriteRenderer>();

        spriteObject.transform.position = position;
        spriteObject.GetComponent<SpriteRenderer>().sprite = sprites.Where(s => s.name == sprite_name).First();
    }

    public static void EraseArrows()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("MovementArrow");
        foreach (GameObject arrow in arrows)
        {
            Destroy(arrow);
        }
    }

}
