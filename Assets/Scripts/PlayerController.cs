using UnityEngine;
using Data;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private PlayerData playerData;

    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;

    private float input;
    private bool onGround;

    private float movementSpeed;
    private float jumpHeight;
    private int jumpAmount;

    private int currentHealth;
    private int maxHealth;
    private int currentOil;
    private int maxOil;

    private void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetBasePlayerStats();
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if (!ctx.started) return; 
        if (!onGround && jumpAmount == 0) return;
        if (jumpAmount != -1) jumpAmount--;
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpHeight);
    }
    
    public void OnMove(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            input = 0f;
            return;
        }
        input = ctx.ReadValue<float>();
        spriteRenderer.flipX = (input > 0) ? false : true;
    }
    
    private void FixedUpdate() {
        rigidbody2d.velocity = new Vector2(input * movementSpeed, rigidbody2d.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            if (collision.contactCount == 0) return;
            if (collision.GetContact(0).point.y > transform.position.y - transform.localScale.y / 2) return;
            onGround = true;
            ResetAirJumps();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            if (collision.contactCount == 0) return;
            if (collision.GetContact(0).point.y > transform.position.y - transform.localScale.y / 2) return;
            onGround = false;
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
        jumpAmount = playerData.WallJumpAmount;
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