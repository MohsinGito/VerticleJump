using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Public Attributes

    [Header("Movement Filds")]
    public float moveSpeed;
    public float jumpHieght;
    public float groundDistance;
    public float yDirectionMax;
    public SpriteRenderer playerBody;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 xDirectionMinMax;
    public GameObject dieEffect;

    [Header("Movement UI")]
    public PlayerMovementButton moveButtonLeft;
    public PlayerMovementButton moveButtonRight;

    [Header("Debugging")]
    [SerializeField] bool canMove;

    #endregion

    #region Private Attributes

    private bool isGameStarted;
    private bool isOnGround;
    private bool isFalling;
    private bool isDead;
    private float currentMaxY;
    private float newYJumpPos;
    private UIManager uiManager;
    private EnvironmentManager environmentManager;
    private Rigidbody2D playerRb;
    private BoxCollider2D playerBx;
    private GameCharacter playerInfo;

    #endregion

    #region Public Methods

    public void Init(GameCharacter _playerInfo, UIManager _uiManager, EnvironmentManager _environmentManager)
    {
        isGameStarted = true;
        playerInfo = _playerInfo;
        uiManager = _uiManager;
        environmentManager = _environmentManager;

        currentMaxY = yDirectionMax;
        newYJumpPos = transform.position.y;
        playerBody.sprite = playerInfo.jumpSprite;
        moveButtonLeft.OnPressed = HorizontalMove;
        moveButtonRight.OnPressed = HorizontalMove;
        playerRb = GetComponent<Rigidbody2D>();
        playerBx = GetComponent<BoxCollider2D>();
        playerRb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void Dead(bool colliodedWithDeadEnd = false)
    {
        isDead = true;
        canMove = false;
        playerBx.isTrigger = false;
        dieEffect.SetActive(true);
        playerBody.sprite = playerInfo.dieSprite;

        DOVirtual.DelayedCall(colliodedWithDeadEnd ? 0.25f : 1.5f, uiManager.GameEnd);
    }

    public void HorizontalMove(float _horizontalDirection)
    {
        playerRb.velocity = new Vector2(_horizontalDirection * moveSpeed * Time.deltaTime, playerRb.velocity.y);
    }

    #endregion

    #region Private Methods

    private void Update()
    {
        if (!canMove || !isGameStarted)
            return;

        if (CanJump())
            Jump();

        CheckBoundaries();
    }

    private void FixedUpdate()
    {
        if (!canMove || !isGameStarted)
            return;

        if (Input.GetAxis("Horizontal") == 0)
            return;

        HorizontalMove(Input.GetAxis("Horizontal"));
    }

    private void Jump()
    {
        newYJumpPos = transform.position.y;
        playerRb.velocity = new Vector2(0, (Vector2.up * jumpHieght).y);
    }

    private bool CanJump()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundLayer);

        if (transform.position.y < newYJumpPos)
        {
            newYJumpPos = transform.position.y;
            playerBody.sprite = playerInfo.idleSprite;
            isFalling = true;
        }

        else if (transform.position.y > newYJumpPos)
        {
            newYJumpPos = transform.position.y;
            playerBody.sprite = playerInfo.jumpSprite;
            isFalling = false;
        }

        playerBx.enabled = isFalling;
        return isOnGround && isFalling;
    }

    private void CheckBoundaries()
    {
        // Maintaining Character Rotation
        transform.rotation = Quaternion.identity;

        if (transform.position.x <= xDirectionMinMax.x)
            transform.position = new Vector2(xDirectionMinMax.y - 0.1f, transform.position.y + 2f);

        if (transform.position.x >= xDirectionMinMax.y)
            transform.position = new Vector2(xDirectionMinMax.x + 0.1f, transform.position.y + 2f);

        if(transform.position.y > currentMaxY)
        {
            currentMaxY += yDirectionMax;
            environmentManager.RepositionEnvironment();
        }
    }

    #endregion

    #region Trigger Events

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (collision.CompareTag("Coin"))
        {
            uiManager.AddRewardScores(true);
            collision.transform.DOMove(collision.transform.position + new Vector3(0, 10f, 0), 1f).
                OnComplete(() => { collision.gameObject.SetActive(false); });
        }

        if (collision.CompareTag("Death Tag"))
            Dead();

        if (collision.CompareTag("DeadEnd"))
            Dead(true);

        if (collision.CompareTag("Environment Patch"))
        {
            environmentManager.RemoveAndSpawnNewPatch();
            collision.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    #endregion

}