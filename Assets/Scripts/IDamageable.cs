using UnityEngine;

public interface IDamageable {
    public void ChangeHealth(int amount);
    public void GiveForce(Vector2 force);
}
