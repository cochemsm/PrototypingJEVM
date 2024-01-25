using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public Vector2 patrolPointA;
    public Vector2 patrolPointB;

    private Vector2 currentPatrolPoint;
    private Vector2 oldPatrolPoint;

    private Vector2 attackDirection = Vector2.right;
    
    private bool right;
    private bool attack;
    public bool startSpecial;

    private GameObject player;

    private void Awake() {
        currentPatrolPoint = patrolPointB;
        oldPatrolPoint = patrolPointA;
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("player");
    }

    public Vector2 MoveToPatrolPoint() {
        if (startSpecial) return Vector2.zero;

        Vector2 temp = (currentPatrolPoint - oldPatrolPoint).normalized * GetComponent<EnemyController>().MovementSpeed;

        if (Math.Abs(transform.position.x - currentPatrolPoint.x) < 0.1f){
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            (currentPatrolPoint, oldPatrolPoint) = (oldPatrolPoint, currentPatrolPoint);
            GetComponent<EnemyController>().FlipChilds();
            right = !right;
            attackDirection = (attackDirection == Vector2.right) ? Vector2.left : Vector2.right;
        }

        if (GetComponent<EnemyController>().ranged) {
            if (Vector2.Distance(transform.position, player.transform.position) < 10) {
                temp = Vector2.zero;
                if (!attack) {
                    attack = true;
                    GetComponent<EnemyController>().Attack(right);
                    StartCoroutine(AttackCooldown(5));
                }
            }

            return temp;
        }

        if (Physics2D.Raycast(transform.position, attackDirection, (!GetComponent<EnemyController>().boss) ? 1 : 2, LayerMask.GetMask("Player")).collider != null) {
            temp = Vector2.zero;
            if (!attack) {
                attack = true;
                GetComponent<EnemyController>().Attack(right);
                StartCoroutine(AttackCooldown(2));
            }
            return temp;
        }

        if (!GetComponent<EnemyController>().specialAttack) return temp;
        if (Physics2D.Raycast(transform.position, attackDirection, 3, LayerMask.GetMask("Player")).collider != null) {
            if (!startSpecial) {
                temp = Vector2.zero;
                startSpecial = true;
                GetComponent<EnemyController>().SpecialAttack();
                return temp;
            }
        }

        return temp;
    }

    public void SpecialHit() {

    }

    private IEnumerator AttackCooldown(int seconds) {
        yield return new WaitForSecondsRealtime(seconds);
        attack = false;
    }

    private IEnumerator AttackIndicator(int seconds) {
        yield return new WaitForSecondsRealtime(seconds);
    }
}