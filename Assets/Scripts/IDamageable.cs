using UnityEngine;

public interface IDamageable {
    public bool ChangeHealth(int amount);
    public void GiveForce(Vector2 force);
}
