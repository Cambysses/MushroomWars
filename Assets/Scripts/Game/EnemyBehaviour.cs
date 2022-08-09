using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite[] sprites;

    private bool isFacingRight = true;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnemyTakeAction()
    {
        direction = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2));
        Debug.Log(direction);
        transform.position += direction;
        Flip();
    }


    private void Flip()
    {
        if (!isFacingRight && direction.x > 0)
        {
            isFacingRight = !isFacingRight;
            sr.sprite = sprites[0];
        }
        else if (isFacingRight && direction.x < 0)
        {
            isFacingRight = !isFacingRight;
            sr.sprite = sprites[1];
        }
    }
}
