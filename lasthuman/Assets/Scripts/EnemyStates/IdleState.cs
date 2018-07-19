using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    // instead of Coroutines...
    private float idleTimer;

    private float idleDuration;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        // every time we enter into idle ..state
        // random idle duration between 1 and 10
        // to make it more independent
        idleDuration = UnityEngine.Random.Range(1, 10);
    }

    public void Execute()
    {
        Idle();

        // if enemy got target
        // start running
        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

    }

    private void Idle()
    {
        // go from movemement state to steady state(set speed to 0)
        enemy.MyAnimator.SetFloat("speed", 0);

        idleTimer += Time.deltaTime;

        // after idleDuration seconds go into Patrol state
        if(idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
}
