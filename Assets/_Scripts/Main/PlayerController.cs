using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Audio;

public class PlayerController : MonoBehaviour
{

    #region Public Attributes

    [Header("Movement Filds")]
    public float moveSpeed;
    public float jumpHieght;
    public float boostJumpHieght;
    public float groundDistance;
    public SpriteRenderer playerBody;
    public Animator playerAnimator;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 xDirectionMinMax;
    public GameObject extraLifeEffect;

    [Header("Movement UI")]
    public PlayerMovementButton moveButtonLeft;
    public PlayerMovementButton moveButtonRight;

    [Header("Debugging")]
    [SerializeField] bool canMove;
    [SerializeField] bool canDie;

    #endregion

    #region Private Attributes

    private bool isDead;
    private bool isFalling;
    private bool isOnGround;
    private bool isGameStarted;
    private float newYJumpPos;
    private UIManager uiManager;
    private EnvironmentManager environmentManager;
    private Rigidbody2D playerRb;
    private CircleCollider2D playerBx;
    private GameCharacter playerInfo;

    [HideInInspector] public bool isInJumpBoost;

    #endregion

    #region Public Methods

    public void Init(GameCharacter _playerInfo, UIManager _uiManager, EnvironmentManager _environmentManager)
    {
        isGameStarted = true;
        playerInfo = _playerInfo;
        uiManager = _uiManager;
        environmentManager = _environmentManager;
        
        newYJumpPos = transform.position.y;
        playerBody.sprite = playerInfo.jumpSprite;
        moveButtonLeft.OnPressed = HorizontalMove;
        moveButtonRight.OnPressed = HorizontalMove;
        playerRb = GetComponent<Rigidbody2D>();
        playerBx = GetComponent<CircleCollider2D>();
        playerRb.bodyType = RigidbodyType2D.Dynamic;
    }

    public bool Dead(bool colliodedWithDeadEnd = false)
    {
        if (!canDie || isInJumpBoost)
            return false;

        if (!uiManager.AllLivesEnded(colliodedWithDeadEnd))
        {
            extraLifeEffect.SetActive(false); // Important Step
            extraLifeEffect.SetActive(true);
            AudioController.Instance.PlayAudio(AudioName.EXTRA_LIFE);
            return false;
        }

        isDead = true;
        canMove = false;
        playerBx.isTrigger = false;
        playerBody.sprite = playerInfo.dieSprite;

        if(colliodedWithDeadEnd)
            playerBody.gameObject.SetActive(false);
        else
            VFXManager.Instance.DisplayVFX("Die Effect", transform.position);

        AudioController.Instance.PlayAudio(AudioName.LOOSE_SFX);
        DOVirtual.DelayedCall(colliodedWithDeadEnd ? 0.25f : 1.5f, uiManager.GameEnd);

        return true;
    }

    public void HorizontalMove(float _horizontalDirection)
    {
        if (!canMove)
            return;

        playerRb.velocity = new Vector2(_horizontalDirection * moveSpeed * Time.deltaTime, playerRb.velocity.y);
    }

    #endregion

    #region Private Methods

    private void FixedUpdate()
    {
        if (!canMove || !isGameStarted)
            return;

        if (Input.GetAxis("Horizontal") != 0)
            HorizontalMove(Input.GetAxis("Horizontal"));

        if (CanJump())
            Jump();

        CheckBoundaries();
    }

    private void Jump()
    {
        if (isInJumpBoost)
            return;

        newYJumpPos = transform.position.y;
        playerAnimator.SetTrigger("Jump");
        playerRb.velocity = new Vector2(0, (Vector2.up * jumpHieght).y);
        AudioController.Instance.PlayAudio(AudioName.JUMP);
    }

    private void BoostJump()
    {
        isInJumpBoost = true;
        newYJumpPos = transform.position.y;
        playerAnimator.SetBool("Boost Jump", true);
        playerRb.velocity = new Vector2(0, (Vector2.up * boostJumpHieght).y);
        AudioController.Instance.PlayAudio(AudioName.BOOST_JUMP);
        VFXManager.Instance.DisplayVFX("Jump Effect", transform.position);
    }

    private bool CanJump()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundLayer);

        if (transform.position.y < newYJumpPos)
        {
            newYJumpPos = transform.position.y;
            playerBody.sprite = playerInfo.idleSprite;
            playerAnimator.ResetTrigger("Jump");
            playerAnimator.SetBool("Boost Jump", false);
            isInJumpBoost = false;
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
            transform.position = new Vector2(xDirectionMinMax.y - 0.1f, transform.position.y + 1f);

        if (transform.position.x >= xDirectionMinMax.y)
            transform.position = new Vector2(xDirectionMinMax.x + 0.1f, transform.position.y + 1f);
    }

    #endregion

    #region Trigger Events

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (isFalling)
        {
            if (collision.CompareTag("Coin"))
            {
                uiManager.AddRewardScores(true);
                collision.transform.DOMove(collision.transform.position + new Vector3(0, 10f, 0), 1f).
                    OnComplete(() => { collision.gameObject.SetActive(false); });
                AudioController.Instance.PlayAudio(AudioName.COIN_COLLECT);
            }

            if (collision.CompareTag("Extra Life"))
            {
                uiManager.AddLife();
                collision.transform.DOMove(collision.transform.position + new Vector3(0, 10f, 0), 1f).
                    OnComplete(() => { collision.gameObject.SetActive(false); });
                collision.transform.DOScale(collision.transform.localScale * 5f, 1f);
                AudioController.Instance.PlayAudio(AudioName.PICKUP_COLLECT);
            }

            if (collision.CompareTag("Ground Obstacle"))
            {
                if(!Dead())
                {
                    VFXManager.Instance.DisplayVFX("Enemy Die Effect", collision.transform.position);
                    collision.gameObject.SetActive(false);
                }
            }

            if (collision.CompareTag("Boost Jump"))
            {
                BoostJump();
            }

            if (collision.CompareTag("Dead End"))
            {
                Dead(true);
            }
        }
    }

    #endregion

}