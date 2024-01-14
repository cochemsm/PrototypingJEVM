using UnityEngine;

namespace Data {
    [CreateAssetMenu(menuName = "Data/PlayerData")]
    public class PlayerData : ScriptableObject {
        [Header("Movement Settings")]
        [SerializeField] private float playerSpeed;
        public float PlayerSpeed => playerSpeed;
        
        [SerializeField] private float playerSpeedMultiplier;
        public float PlayerSpeedMultiplier => playerSpeedMultiplier;


        [Header("Jump Settings")]
        [SerializeField] private float jumpHeight;
        public float JumpHeight => jumpHeight;

        [SerializeField] private float jumpHeightMultiplier;
        public float JumpHeightMultiplier => jumpHeightMultiplier;

        [Tooltip("The amount of time the player can jump in the air (infinite = -1)")]
        [SerializeField] private int doubleJumpAmount;
        public int DoubleJumpAmount => doubleJumpAmount;

        [Tooltip("The amount of time the player can jump on walls (infinite = -1)")]
        [SerializeField] private int wallJumpAmount;
        public int WallJumpAmount => wallJumpAmount;


        [Header("Health Settings")]
        [SerializeField] private int health;
        public int Health => health;
        
        [SerializeField] private int maxHealth;
        public int MaxHealth => maxHealth;


        [Header("Oil Settings")]
        [SerializeField] private int oil;
        public int Oil => oil;

        [SerializeField] private int maxOil;
        public int MaxOil => maxOil;

        [Header("Starting Settings")]
        [SerializeField] private Vector2 startPoint;
        public Vector2 StartPoint => startPoint;

        [SerializeField] private int damage;
        public int Damage => damage;
    }
}