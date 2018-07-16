using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{
    // reference to enemy
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        
    }

    public void Execute()
    {
        if (enemy.Target != null)
        {
            enemy.Move();
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }
}
