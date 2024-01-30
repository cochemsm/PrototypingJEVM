using System;
using UnityEngine;

public interface IDamageable {
    public int ChangeHealth(int amount);
    public void GiveForce(Vector2 force);
}
