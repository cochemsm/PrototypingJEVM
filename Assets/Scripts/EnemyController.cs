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
    private bool startSpecial;

    [SerializeField] private string forkliftColor;
    [SerializeField] private bool liftable;
    [SerializeField] private bool secondaryAttack;
    [SerializeField] private bool basicAttackDownwards;
    [SerializeField] public bool specialAttack;
    [SerializeField] public bool ranged;
    [SerializeField] private bool boss;

    [SerializeField] private GameObject projectile;

    public int Damage => damage;
    public float MovementSpeed => movementSpeed;

    private void Awake() {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        attackHitBox = transform.GetChild(0).gameObject;
        SetBaseStats();
        StartCoroutine(EngineSound());
    }

    private void Update() {
        if (specialAttack) { 
            
        }
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
        if (boss) GameManager.Instance.SetBossHealth((float) currentHealth / maxHealth);
        if (currentHealth == 0) Death();
        stunned = true;
        StopCoroutine(Stun(1));
        return true;
    }

    public void GiveForce(Vector2 force) {
        rigidbody2d.velocity = liftable ? force : new Vector2(force.x, 0);
    }

    private IEnumerator Stun(float time) {
        yield return new WaitForSeconds(time);
        stunned = false;
    }

    public void Attack(bool side) {
        right = side;
        animator.Play(forkliftColor + "_basic_attack");
        attackHitBox.GetComponent<AttackScript>().force = new Vector2(right ? -3 : 3, basicAttackDownwards ? -1 : 6);
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
        stunReset = StartCoroutine(Stun(1));
    }

    private void ActivateAttackHitbox() {
        attackHitBox.SetActive(true);
    }

    public void SpecialAttack() {
        animator.Play(forkliftColor + "_special_attack_start");
        startSpecial = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!startSpecial) return;
        if (!collision.gameObject.CompareTag("player")) return;

        collision.gameObject.GetComponent<PlayerController>().GiveForce(new Vector2(0, 7));
    }

    private void RangedAttack() {
        float scaleX = transform.localScale.x / transform.lossyScale.x / 2 + projectile.transform.localScale.x / projectile.transform.lossyScale.x / 2;
        float scaleY = transform.localScale.x / transform.lossyScale.x / 4;
        Instantiate(projectile, new Vector3(transform.position.x + (right ? -scaleX : scaleX), transform.position.y - scaleY, 0), new Quaternion(0, 0, right ? 180 : 0, 0));
    }

    public void FlipChilds() {
        attackHitBox.transform.localPosition = new Vector2(attackHitBox.transform.localPosition.x * -1, attackHitBox.transform.localPosition.y);
        transform.GetChild(2).localPosition = new Vector2(transform.GetChild(2).localPosition.x * -1, transform.GetChild(2).localPosition.y);
    }

    private void Death() {
        AudioManager.Instance.PlaySound("8bit_bomb_explosion");
        ParticleSystem death = transform.GetChild(2).GetComponent<ParticleSystem>();
        death.Play();
        death.transform.SetParent(null, true);
        Destroy(gameObject);
    }

    private IEnumerator EngineSound() {
        AudioManager.Instance.PlaySound("engine_sound");
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(EngineSound());
    }
}
