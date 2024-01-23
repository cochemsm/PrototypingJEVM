using System.Collections;
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

    private bool right;
    private bool stunned;

    [SerializeField] private string forkliftColor;
    [SerializeField] private bool liftable;
    [SerializeField] private bool secondaryAttack;

    public int Damage => damage;
    public float MovementSpeed => movementSpeed;

    private void Awake() {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        attackHitBox = transform.GetChild(0).gameObject;
        SetBaseStats();
    }

    private void Update() {
        if (stunned) return; 
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
    
    public bool ChangeHealth(int value) {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        if (currentHealth == 0) Death();
        stunned = true;
        StopCoroutine(Stun());
        return true;
    }

    public void GiveForce(Vector2 force) {
        rigidbody2d.velocity = liftable ? force : new Vector2(force.x, 0);
    }

    private IEnumerator Stun() {
        yield return new WaitForSeconds(1);
        stunned = false;
    }

    public void Attack(bool right) {
        right = right;
        animator.Play(forkliftColor + "_basic_attack");
        attackHitBox.GetComponent<AttackScript>().force = new Vector2(right ? -3 : 3, 6);
    }

    private Coroutine stunReset;
    public void StartCombo(bool hit) {
        if (!secondaryAttack) return;
        if (hit) {
            animator.Play(forkliftColor + "_secondary_attack");
            attackHitBox.GetComponent<AttackScript>().force = new Vector2(right ? -1 : 1, -1);
        } else {
            animator.Play(forkliftColor + "_basic_attack_cancel");
        }
        
        if (stunReset != null) StopCoroutine(stunReset);
        stunReset = StartCoroutine(Stun());
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
