using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    // instead of Coroutines...
    private float idleTimer;

    private float idleDuration = 5f;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Idle();
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
