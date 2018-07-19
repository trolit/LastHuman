using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private float patrolTimer;
    private float patrolDuration;
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        // every time we go into Patrol State
        // randomize patrol Duration time between
        // 1 and 10 
        this.enemy = enemy;
        patrolDuration = UnityEngine.Random.Range(1, 10);
    }

    public void Execute()
    {
        Patrol();

        enemy.Move();

        // if enemy got target and is in melee range then go into MeleeState
        if(enemy.Target != null && enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {
        /*
         * Now we check edges by position in Enemy
         * script, in function "Move", else if 
         * contains ChangeDirection function
         * so no need in doing the same over here
         * ....soooo commenting this one
         
        // if enemy reached edge 
        // go in opposite direction
        if(other.tag == "Edge")
        {
            enemy.ChangeDirection();
        }
        */
    }

    private void Patrol()
    {
        patrolTimer += Time.deltaTime;

        // after patrolDuration seconds go into Idle state
        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
