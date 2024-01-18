using UnityEngine;
using Data;

public class EnemyController : MonoBehaviour {

    [SerializeField] private EnemyData enemyData;

    private Animator animator;
    private Rigidbody2D rigidbody2d;

    private GameObject attackHitBox;

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
        attackHitBox = transform.GetChild(0).gameObject;
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
            Destroy(gameObject);
            return;
        }
        if (currentHealth + value >= maxHealth) {
            currentHealth = maxHealth;
            return;
        }
        currentHealth += value;
    }

    public void Attack() {
        animator.SetTrigger(StartAttack);
        Invoke(nameof(ActivateAttackHitbox), 0.1f);
    }

    private void ActivateAttackHitbox() {
        rigidbody2d.MovePosition(rigidbody2d.position);
        attackHitBox.SetActive(true);
    }

    public void FlipChilds() {
        attackHitBox.transform.localPosition = new Vector2(attackHitBox.transform.localPosition.x * -1, attackHitBox.transform.localPosition.y);
        transform.GetChild(2).localPosition = new Vector2(transform.GetChild(2).localPosition.x * -1, transform.GetChild(2).localPosition.y);
    }
}
