using UnityEngine;

public class BossBarActivationScript : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        GameManager.Instance.ActivateBossbar();
        MusicManager.Instance.ChangeMusic("Boss Battle");
        Destroy(gameObject);
    }
}