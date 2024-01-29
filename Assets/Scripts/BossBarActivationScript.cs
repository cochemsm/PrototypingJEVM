using UnityEngine;

public class BossBarActivationScript : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("player")) return;
        GameManager.Instance.ActivateBossbar();
        AudioManager.Instance.PlayMusic(AudioManager.BossMusic);
        Destroy(gameObject);
    }
}