using System;
using UnityEngine;

public class Rocket : MonoBehaviour {
    private Rigidbody2D rigidbody2d;
    [SerializeField] private int damage;

    private void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(transform.rotation.z == 0 ? 4 : -4, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.GetComponent<IDamageable>() != null)
            transform.GetComponent<IDamageable>().ChangeHealth(-damage);
        Destroy(gameObject);
    }
}