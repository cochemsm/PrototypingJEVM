using UnityEngine;
using Data;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private PlayerData playerData;

    private Rigidbody2D rigidbody2d;

    private float input;
    private bool canJump;

    private int health;

    private void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if (!canJump) return;
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, playerData.JumpHeight);
    }
    
    public void OnMove(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            input = 0f;
            return;
        }
        input = ctx.ReadValue<float>(); 
    }
    
    private void FixedUpdate() {
        rigidbody2d.velocity = new Vector2(input * playerData.PlayerSpeed, rigidbody2d.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        canJump = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        canJump = false;
    }
}