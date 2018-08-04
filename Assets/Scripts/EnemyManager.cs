using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public LayerMask blockLayer;

    private Rigidbody2D rbody;

    private float moveSpeed = 1;

    public enum MOVE_DIR
    {
        LEFT,
        RIGHT,
    };

    private MOVE_DIR moveDirection = MOVE_DIR.LEFT;

    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        bool isBlock;

        switch (moveDirection)
        {
            case MOVE_DIR.LEFT:
                rbody.velocity = new Vector2(moveSpeed * -1, rbody.velocity.y);
                transform.localScale = new Vector2(1, 1);
                isBlock = Physics2D.Linecast(
                        new Vector2(transform.position.x, transform.position.y + 0.5f),
                        new Vector2(transform.position.x - 0.3f, transform.position.y + 0.5f),
                        blockLayer);

                if (isBlock)
                {
                    moveDirection = MOVE_DIR.RIGHT;
                }
                break;

            case MOVE_DIR.RIGHT:
                rbody.velocity = new Vector2(moveSpeed, rbody.velocity.y);
                transform.localScale = new Vector2(-1, 1);
                isBlock = Physics2D.Linecast(
                        new Vector2(transform.position.x, transform.position.y + 0.5f),
                        new Vector2(transform.position.x + 0.3f, transform.position.y + 0.5f),
                        blockLayer);

                if (isBlock)
                {
                    moveDirection = MOVE_DIR.LEFT;
                }
                break;
        }
    }
}
