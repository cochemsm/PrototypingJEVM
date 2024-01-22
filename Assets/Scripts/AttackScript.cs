using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    private bool player;
    private int damage;

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
            if (player) {
                if (hit.CompareTag("enemy")) {
                    hit.gameObject.GetComponent<EnemyController>().ChangeHealth(-damage);
                }
            } else {
                if (hit.CompareTag("player")) {
                    hit.gameObject.GetComponent<PlayerController>().ChangeHealth(-damage);
                }
            }
        }
        if (player) gameObject.GetComponentInParent<PlayerController>().StartCombo();
        gameObject.SetActive(false);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
