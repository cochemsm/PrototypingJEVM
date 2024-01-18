using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public Vector2 patrolPointA;
    public Vector2 patrolPointB;

    private Vector2 currentPatrolPoint;
    private Vector2 oldPatrolPoint;

    private Vector2 attackDirection = Vector2.right;

    private void Awake() {
        currentPatrolPoint = patrolPointB;
        oldPatrolPoint = patrolPointA;
    }

    public Vector2 MoveToPatrolPoint() {
        Vector2 temp = (currentPatrolPoint - oldPatrolPoint).normalized * GetComponent<EnemyController>().MovementSpeed;

        if (Math.Abs(transform.position.x - currentPatrolPoint.x) < 0.1f){
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            (currentPatrolPoint, oldPatrolPoint) = (oldPatrolPoint, currentPatrolPoint);
            GetComponent<EnemyController>().FlipChilds();
            attackDirection = (attackDirection == Vector2.right) ? Vector2.left : Vector2.right;
        }

        if (Physics2D.Raycast(transform.position, attackDirection, 2, LayerMask.GetMask("Player")).collider != null) {
            temp = Vector2.zero;
            GetComponent<EnemyController>().Attack();
        }

        return temp;
    }

    public void MoveTillTheEnd() {
        
    }

    public void FindObstacle() {
        
    }
}