using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    public GameObject gameManager;

    public LayerMask blockLayer;

    private Rigidbody2D rbody;
    private Animator animator;

    private const float MOVE_SPEED = 3;
    private float moveSpeed;
    private float jumpPower = 400;
    private bool goJump = false;
    private bool canJump = false;
    private bool usingButtons = false;

    public enum MOVE_DIR
    {
        STOP,
        LEFT,
        RIGHT,
    };

    private MOVE_DIR moveDirection = MOVE_DIR.STOP;

    public AudioClip jumpSE;
    public AudioClip getSE;
    public AudioClip stampSE;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        audioSource = gameManager.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        canJump =
            Physics2D.Linecast(
                transform.position - (transform.right * 0.3f),
                transform.position - (transform.up * 0.1f),
                blockLayer) ||
            Physics2D.Linecast(
                transform.position + (transform.right * 0.3f),
                transform.position - (transform.up * 0.1f),
                blockLayer);

        animator.SetBool("onGround", canJump);

        if (!usingButtons)
        {
            float x = Input.GetAxisRaw("Horizontal");

            if (x == 0)
            {
                moveDirection = MOVE_DIR.STOP;
            }
            else
            {
                if (x < 0)
                {
                    moveDirection = MOVE_DIR.LEFT;
                }
                else
                {
                    moveDirection = MOVE_DIR.RIGHT;
                }
            }

            if (Input.GetKeyDown("space"))
            {
                PushJumpButton();
            }
        }
    }

    private void FixedUpdate()
    {
        switch (moveDirection)
        {
            case MOVE_DIR.STOP:
                moveSpeed = 0;
                break;
            case MOVE_DIR.LEFT:
                moveSpeed = MOVE_SPEED * -1;
                transform.localScale = new Vector2(-1, 1);
                break;
            case MOVE_DIR.RIGHT:
                moveSpeed = MOVE_SPEED;
                transform.localScale = new Vector2(1, 1);
                break;
            default:
                break;
        }

        rbody.velocity = new Vector2(moveSpeed, rbody.velocity.y);

        if (goJump)
        {
            audioSource.PlayOneShot(jumpSE);
            rbody.AddForce(Vector2.up * jumpPower);
            goJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.GetComponent<GameManager>().gameMode != GameManager.GAME_MODE.PLAY)
        {
            return;
        }

        if (collision.gameObject.tag == "Trap")
        {
            gameManager.GetComponent<GameManager>().GameOver();
            DestroyPlayer();
        }
        if (collision.gameObject.tag == "Goal")
        {
            gameManager.GetComponent<GameManager>().GameClear();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (transform.position.y > collision.gameObject.transform.position.y + 0.4f)
            {
                audioSource.PlayOneShot(stampSE);
                rbody.velocity = new Vector2(rbody.velocity.x, 0);
                rbody.AddForce(Vector2.up * jumpPower);
                collision.gameObject.GetComponent<EnemyManager>().DestroyEnemy();
            }
            else
            {
                gameManager.GetComponent<GameManager>().GameOver();
                DestroyPlayer();
            }
        }
        if (collision.gameObject.tag == "Orb")
        {
            audioSource.PlayOneShot(getSE);
            collision.gameObject.GetComponent<OrbManager>().GetOrb();
        }
    }

    public void PushLeftButton()
    {
        moveDirection = MOVE_DIR.LEFT;
        usingButtons = true;
    }

    public void PushRightButton()
    {
        moveDirection = MOVE_DIR.RIGHT;
        usingButtons = true;
    }

    public void ReleaseMoveButton()
    {
        moveDirection = MOVE_DIR.STOP;
        usingButtons = false;
    }

    public void PushJumpButton()
    {
        if (canJump)
        {
            goJump = true;
        }
    }

    private void DestroyPlayer()
    {
        gameManager.GetComponent<GameManager>().gameMode = GameManager.GAME_MODE.GAMEOVER;

        Destroy(GetComponent<CircleCollider2D>());
        Destroy(GetComponent<BoxCollider2D>());

        Sequence animSet = DOTween.Sequence();
        animSet.Append(transform.DOLocalMoveY(1.0f, 0.2f).SetRelative());
        animSet.Append(transform.DOLocalMoveY(-10.0f, 1.0f).SetRelative());

        Destroy(this.gameObject, 1.2f);
    }
}
