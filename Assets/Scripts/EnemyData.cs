using UnityEngine;

namespace Data {
    [CreateAssetMenu(menuName = "Data/EnemyData")]
    public class EnemyData : ScriptableObject {
        [Header("Movement Settings")]
        [SerializeField] private float playerSpeed;
        public float PlayerSpeed => playerSpeed;

        [SerializeField] private float playerSpeedMultiplier;
        public float PlayerSpeedMultiplier => playerSpeedMultiplier;

        [Header("Health Settings")]
        [SerializeField] private int maxHealth;
        public int MaxHealth => maxHealth;

        [Header("Starting Settings")]
        [SerializeField] private int damage;
        public int Damage => damage;
    }
}