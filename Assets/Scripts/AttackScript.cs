using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    private bool player;
    private int damage;
    public Vector2 force;

    private void Awake() {
        if (gameObject.GetComponentInParent<PlayerController>()) {
            player = true;
            damage = gameObject.GetComponentInParent<PlayerController>().Damage;
        } else {
            damage = gameObject.GetComponentInParent<EnemyController>().Damage;
        }
    }

    private void OnEnable() {
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D {
            layerMask = LayerMask.GetMask("Enemy")
        };
        GetComponent<BoxCollider2D>().OverlapCollider(filter, results);
        foreach (var hit in results) {
            if (player ? hit.CompareTag("enemy") : hit.CompareTag("player")) {
                hit.gameObject.GetComponent<IDamageable>().ChangeHealth(-damage);
                hit.gameObject.GetComponent<IDamageable>().GiveForce(force);
            }
        }
        if (player) gameObject.GetComponentInParent<PlayerController>().StartCombo();
        gameObject.SetActive(false);
    }
}
