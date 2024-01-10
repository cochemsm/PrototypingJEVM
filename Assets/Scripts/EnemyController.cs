using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();    
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) animator.SetTrigger("StartAttack");
    }
}
