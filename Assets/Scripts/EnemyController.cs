using UnityEngine;
using Data;

public class EnemyController : MonoBehaviour {

    [SerializeField] private EnemyData enemyData;

    private Animator animator;
    private Rigidbody2D rigidbody2d;

    private int currentHealth;
    private int maxHealth;
    private int damage;
    private float movementSpeed;
    private static readonly int StartAttack = Animator.StringToHash("StartAttack");

    public int Damage => damage;
    public float MovementSpeed => movementSpeed;

    private void Awake() {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        SetBaseStats();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) animator.SetTrigger(StartAttack);
        rigidbody2d.velocity = new Vector2(GetComponent<EnemyMovement>().MoveToPatrolPoint().x, rigidbody2d.velocity.y);
    }

    private void SetBaseStats() {
        ResetSpeedModifier();

        maxHealth = enemyData.MaxHealth;
        currentHealth = maxHealth;
        damage = enemyData.Damage;
    }

    public void ApplySpeedModifier() {
        movementSpeed *= enemyData.PlayerSpeedMultiplier;
    }

    public void ResetSpeedModifier() {
        movementSpeed = enemyData.PlayerSpeed;
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
}
