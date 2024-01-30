using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    private bool player;
    private int damage;
    public Vector2 force;

    private void Start() {
        if (gameObject.GetComponentInParent<PlayerController>()) {
            player = true;
            damage = gameObject.GetComponentInParent<PlayerController>().Damage;
            print(damage);
        } else {
            damage = gameObject.GetComponentInParent<EnemyController>().Damage;
        }
    }

    public void Attack() {
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D {
            layerMask = LayerMask.GetMask("Enemy")
        };
        bool hit = false;
        GetComponent<BoxCollider2D>().OverlapCollider(filter, results);
        foreach (var hitTarget in results) {
            if (player ? hitTarget.CompareTag("enemy") : hitTarget.CompareTag("player")) {
                IDamageable target = hitTarget.gameObject.GetComponent<IDamageable>();
                if (target != null) hit = true;
                int targetHealth = target.ChangeHealth(-damage);
                target.GiveForce(force);
                if (targetHealth == 0 && player) gameObject.GetComponentInParent<PlayerController>().ChangeOil(1);
            }
        }

        if (player) {
            gameObject.GetComponentInParent<PlayerController>().StartCombo();
        } else if (!player) {
            GetComponentInParent<EnemyController>().StartCombo(hit);
        }
    }
}
