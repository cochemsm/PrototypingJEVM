using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public Vector2 patrolPointA;
    public Vector2 patrolPointB;

    private Vector2 currentPatrolPoint;
    private Vector2 oldPatrolPoint;

    private void Awake() {
        currentPatrolPoint = patrolPointB;
        oldPatrolPoint = patrolPointA;
    }

    public Vector2 MoveToPatrolPoint() {
        Vector2 temp = (currentPatrolPoint - oldPatrolPoint).normalized * GetComponent<EnemyController>().MovementSpeed;
        
        if (Math.Abs(transform.position.x - currentPatrolPoint.x) < 0.1f){
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            (currentPatrolPoint, oldPatrolPoint) = (oldPatrolPoint, currentPatrolPoint);
            GetComponent<EnemyController>().FlipAttackHitbox();
        }
       
        
        return temp;
    }

    public void MoveTillTheEnd() {
        
    }

    public void FindObstacle() {
        
    }
}