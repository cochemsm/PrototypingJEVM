using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
    private bool player = false;
    private int damage;

    private void Awake() {
        if (gameObject.GetComponentInParent<PlayerController>()) {
            player = true;
            damage = gameObject.GetComponentInParent<PlayerController>().Damage;
        } else {
            damage = gameObject.GetComponentInParent<EnemyController>().Damage;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (player) {
            if (collision.gameObject.tag == "enemy") {
                collision.gameObject.GetComponent<EnemyController>().ChangeHealth(-damage);
            }
        } else {
            if (collision.gameObject.tag == "player") {
                collision.gameObject.GetComponent<PlayerController>().ChangeHealth(-damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        // print(other.gameObject.name);
    }

    private void OnEnable() {
        Invoke("DisableHitbox",0.2f);
    }

    private void DisableHitbox() {
        gameObject.SetActive(false);
    }
}
