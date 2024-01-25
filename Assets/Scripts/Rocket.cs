using UnityEngine;

public class Rocket : MonoBehaviour {
    private Rigidbody2D rigidbody2d;

    private void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(transform.rotation.z == 0 ? -1 : 1, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Destroy(gameObject);
    }
}