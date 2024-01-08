using UnityEngine;

namespace Data {
    [CreateAssetMenu(menuName = "Data/PlayerData")]
    public class PlayerData : ScriptableObject {
        public float PlayerSpeed => playerSpeed;
        [SerializeField] private float playerSpeed;
        
        public float PlayerSpeedMultiplier => playerSpeedMultiplier;
        [SerializeField] private float playerSpeedMultiplier;

        public float JumpHeight => jumpHeight;
        [SerializeField] private float jumpHeight;

        public float JumpHeightMultiplier => jumpHeightMultiplier;
        [SerializeField] private float jumpHeightMultiplier;
        
        public int Health => health;
        [SerializeField] private int health;
        
        public int MaxHealth => maxHealth;
        [SerializeField] private int maxHealth;

        public int Oil => oil;
        [SerializeField] private int oil;

        public int MaxOil => maxOil;
        [SerializeField] private int maxOil;

        public void ChangeHealth(int value) {
            if (health + value <= 0) {
                health = 0;
                return;
            }
            if (health + value >= maxHealth) {
                health = maxHealth;
                return;
            }
            health += value;
        }
        
        public void ChangeOil(int value) {
            if (oil + value <= 0) {
                oil = 0;
                return;
            }
            if (oil + value >= maxOil) {
                oil = maxOil;
                return;
            }
            oil += value;
        }
    }
}