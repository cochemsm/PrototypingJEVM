using UnityEngine;
using Data;

public class EnemyController : MonoBehaviour {

    [SerializeField] private EnemyData enemyData;

    private Animator animator;

    private int currentHealth;
    private int maxHealth;
    private int damage;
    private float movementSpeed;

    public int Damage;

    private void Awake() {
        animator = GetComponent<Animator>();
        SetBaseStats();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) animator.SetTrigger("StartAttack");
    }

    private void SetBaseStats() {
        ResetSpeedModifier();

        maxHealth = enemyData.MaxHealth;
        currentHealth = maxHealth;
        damage = enemyData.Damage;
    }

    public void ApplySpeedModifier() {
        movementSpeed = movementSpeed * enemyData.PlayerSpeedMultiplier;
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
