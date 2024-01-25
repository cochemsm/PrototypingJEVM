using UnityEngine;

public class BossBarActivationScript : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("player")) return;
        GameManager.Instance.ActivateBossbar();
        MusicManager.Instance.ChangeMusic("Boss Battle");
        Destroy(gameObject);
    }
}