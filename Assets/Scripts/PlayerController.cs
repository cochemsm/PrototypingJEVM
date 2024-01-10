using UnityEngine;
using Data;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private PlayerData playerData;

    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float input;
    private bool onGround;
    private bool onWall;
    private bool onWallRight;

    private float movementSpeed;
    private float jumpHeight;
    private int jumpAmount;
    private int walljumpAmount;

    private int currentHealth;
    private int maxHealth;
    private int currentOil;
    private int maxOil;

    private void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        SetBasePlayerStats();
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if (onWall) {
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidbody2d.velocity = new Vector2(0, jumpHeight);
            input = onWallRight ? -1 : 1;
            return;
        }
        if (!ctx.started) return;
        if (!onGround && jumpAmount == 0) return;
        if (jumpAmount != -1) jumpAmount--;
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpHeight);
    }
    
    public void OnMove(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            input = 0f;
            animator.SetTrigger("StopWalking");
            return;
        }
        input = ctx.ReadValue<float>();
        spriteRenderer.flipX = (input > 0) ? false : true;
        animator.SetTrigger("StartWalking");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            animator.SetTrigger("StartPunch");
        }
    }

    private void FixedUpdate() {
        rigidbody2d.velocity = new Vector2(input * movementSpeed, rigidbody2d.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            if (Physics2D.Raycast(transform.position - new Vector3(transform.localScale.x / transform.lossyScale.x / 2, 0, 0), Vector2.right, LayerMask.NameToLayer("Ground")).distance == 0 && walljumpAmount != 0) {
                onWall = true;
                onWallRight = false;
                rigidbody2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }
            if (Physics2D.Raycast(transform.position + new Vector3(transform.localScale.x / transform.lossyScale.x / 2, 0, 0), Vector2.left, LayerMask.NameToLayer("Ground")).distance == 0 && walljumpAmount != 0) {
                onWall = true;
                onWallRight = true;
                rigidbody2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }

            if (collision.contactCount == 0) return;
            if (collision.GetContact(0).point.y > transform.position.y - transform.localScale.y / transform.lossyScale.y / 2) return;

            onGround = true;
            ResetAirJumps();
            ResetWallJumps();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            if (onWall) {
                onWall = false;
                if (walljumpAmount != -1) walljumpAmount--;
                rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            if (onGround) onGround = false;
        }
    }

    private void SetBasePlayerStats() {
        ResetSpeedModifier();
        ResetJumpModifier();
        ResetAirJumps();
        ResetWallJumps();
        currentHealth = playerData.Health;
        maxHealth = playerData.MaxHealth;
        currentOil = playerData.Oil;
        maxOil = playerData.MaxOil;
    }

    public void ApplySpeedModifier() {
        movementSpeed = movementSpeed * playerData.PlayerSpeedMultiplier;
    }

    public void ResetSpeedModifier() {
        movementSpeed = playerData.PlayerSpeed;
    }

    public void ApplyJumpHeightModifier() {
        jumpHeight = jumpHeight * playerData.JumpHeightMultiplier;
    }

    public void ResetJumpModifier() {
        jumpHeight = playerData.JumpHeight;
    }

    private void ResetAirJumps() {
        jumpAmount = playerData.DoubleJumpAmount;
    }

    private void ResetWallJumps() {
        walljumpAmount = playerData.WallJumpAmount;
    }

    public void ChangeHealth(int value) {
        if (currentHealth + value <= 0) {
            currentHealth = 0;
            return;
        }
        if (currentHealth + value >= maxHealth) {
            currentHealth = maxHealth;
            return;
        }
        currentHealth += value;
    }

    public void ChangeOil(int value) {
        if (currentOil + value <= 0) {
            currentOil = 0;
            return;
        }
        if (currentOil + value >= maxOil) {
            currentOil = maxOil;
            return;
        }
        currentOil += value;
    }
}