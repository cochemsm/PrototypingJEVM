using UnityEngine;
using Data;

public class EnemyController : MonoBehaviour, IDamageable {

    [SerializeField] private EnemyData enemyData;

    private Animator animator;
    private Rigidbody2D rigidbody2d;

    private GameObject attackHitBox;

    private int currentHealth;
    private int maxHealth;
    private int damage;
    private float movementSpeed;

    [SerializeField] private string forkliftColor;

    public int Damage => damage;
    public float MovementSpeed => movementSpeed;

    private void Awake() {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        attackHitBox = transform.GetChild(0).gameObject;
        SetBaseStats();
    }

    private void Update() {
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
            Death();
            return;
        }
        if (currentHealth + value >= maxHealth) {
            currentHealth = maxHealth;
            return;
        }
        currentHealth += value;
    }

    public void Attack() {
        animator.Play(forkliftColor + "_basic_attack");
    }

    private void ActivateAttackHitbox() {
        attackHitBox.SetActive(true);
    }

    public void FlipChilds() {
        attackHitBox.transform.localPosition = new Vector2(attackHitBox.transform.localPosition.x * -1, attackHitBox.transform.localPosition.y);
        transform.GetChild(2).localPosition = new Vector2(transform.GetChild(2).localPosition.x * -1, transform.GetChild(2).localPosition.y);
    }

    private void Death() {
        ParticleSystem death = transform.GetChild(2).GetComponent<ParticleSystem>();
        death.Play();
        death.transform.SetParent(null, true);
        Destroy(gameObject);
    }
}
