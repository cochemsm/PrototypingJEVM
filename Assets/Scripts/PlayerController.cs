using System;
using System.Collections;
using UnityEngine;
using Data;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IDamageable {
    [SerializeField] private PlayerData playerData;

    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private Camera myCamera;
    
    private float input;
    private bool onGround;
    private bool onWall;
    private bool onWallRight;
    private bool death;
    private int combo;
    private bool comboCooldown;
    private bool jumpedWall;

    private float movementSpeed;
    private float jumpHeight;
    private int jumpAmount;
    private int walljumpAmount;

    private int currentHealth;
    private int maxHealth;
    private int currentOil;
    private int maxOil;
    private int damage;
    
    private Coroutine comboReset;
    
    public int CurrentHealth => currentHealth;
    public int CurrentOil => currentOil;
    public int Damage => damage;

    private Vector2 currentRespawnPoint;

    private GameObject attackHitBox;

    private void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        SetBasePlayerStats();
        attackHitBox = transform.GetChild(0).gameObject;
    }

    private void Start() {
        GameManager.Instance.SetHealthbar((float) currentHealth / maxHealth);
        GameManager.Instance.SetOilbar((float) currentOil / maxOil);
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if (onWall) {
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidbody2d.velocity = new Vector2(0, jumpHeight);
            input = onWallRight ? -1 : 1;
            jumpedWall = true;
            StartCoroutine(JumpedFromWall());
            return;
        }
        if (!ctx.started) return;
        if (!onGround && jumpAmount == 0) return;
        if (jumpAmount != -1) jumpAmount--;
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpHeight);
        animator.Play("main_character_jump");
    }
    
    public void OnMove(InputAction.CallbackContext ctx) {
        if (jumpedWall) return;
        if (ctx.canceled) {
            input = 0f;
            if (!onWall) animator.Play("main_character_idle");
            return;
        }
        input = ctx.ReadValue<float>();
        spriteRenderer.flipX = input < 0;
        attackHitBox.transform.localPosition = new Vector2(attackHitBox.transform.localPosition.x * -1, attackHitBox.transform.localPosition.y);
        animator.Play("main_character_walking");
    }

    private IEnumerator JumpedFromWall() {
        yield return new WaitForSeconds(1);
        jumpedWall = false;
    }

    public void OnRespawn(InputAction.CallbackContext ctx) {
        if (!death) return;
        death = false;
        Respawn();
    }

    private void Update() {
        myCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    private void FixedUpdate() {
        rigidbody2d.velocity = new Vector2(input * movementSpeed, rigidbody2d.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, LayerMask.GetMask("Ground"));
        if (hit.collider) {
            if ((int) hit.distance == 0 && walljumpAmount != 0) {
                onWall = true;
                onWallRight = false;
                rigidbody2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                animator.Play("main_character_on_wall");
                return;
            }
        }
        hit = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, LayerMask.GetMask("Ground"));
        if (hit.collider) {
            if ((int) hit.distance == 0 && walljumpAmount != 0) {
                onWall = true;
                onWallRight = true;
                rigidbody2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                animator.Play("main_character_on_wall");
                return;
            }
        }

        if (collision.contactCount == 0) return;
        if (collision.GetContact(0).point.y > transform.position.y - transform.localScale.y / transform.lossyScale.y / 2) return;

        onGround = true;
        ResetAirJumps();
        ResetWallJumps();
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
        
        if (onWall) {
            onWall = false;
            if (walljumpAmount != -1) walljumpAmount--;
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        if (onGround) onGround = false;
    }

    private void SetBasePlayerStats() {
        ResetSpeedModifier();
        ResetJumpModifier();
        ResetAirJumps();
        ResetWallJumps();

        maxHealth = playerData.MaxHealth;
        maxOil = playerData.MaxOil;
        damage = playerData.Damage;

        SetRespawnPoint(playerData.StartPoint);

        Respawn();
    }

    public void ApplySpeedModifier() {
        movementSpeed *= playerData.PlayerSpeedMultiplier;
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
        jumpAmount = playerData.DoubleJumpAmount + 1;
    }

    private void ResetWallJumps() {
        walljumpAmount = playerData.WallJumpAmount;
    }

    public void ChangeHealth(int value) {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        if (currentHealth == 0) Death();

        GameManager.Instance.SetHealthbar((float) currentHealth / maxHealth);
    }

    public void ChangeOil(int value) {
        currentOil = Mathf.Clamp(currentOil + value, 0, maxOil);
        GameManager.Instance.SetOilbar((float) currentOil / maxOil);
    }

    public void SetRespawnPoint(Vector2 point) {
        currentRespawnPoint = point;
    }

    public void Respawn() {
        transform.position = currentRespawnPoint;
        currentHealth = maxHealth;
        currentOil = maxOil;
    }

    public void OnAttack(InputAction.CallbackContext ctx) {
        if (comboCooldown) return;
        if (!ctx.started) return;
        switch (combo) {
            case 1:
                animator.Play("main_character_punch2");
                return;
            case 2:
                animator.Play("main_character_punch3");
                return;
            default:
                animator.Play("main_character_punch1");
                break;
        }
    }

    private void Death() {
        death = true;
    }

    public void StartCombo() {
        combo += 1;
        if (combo == 3) {
            combo = 0;
            comboCooldown = true;
            StartCoroutine(ComboCooldown());
            return;
        }
        if (comboReset != null) StopCoroutine(comboReset);
        comboReset = StartCoroutine(ResetCombo());
    }

    private IEnumerator ComboCooldown() {
        yield return new WaitForSeconds(2);
        comboCooldown = false;
        animator.Play("main_character_idle");
    }
    
    private IEnumerator ResetCombo() {
        yield return new WaitForSeconds(1);
        combo = 0;
        animator.Play("main_character_idle");
    }
}