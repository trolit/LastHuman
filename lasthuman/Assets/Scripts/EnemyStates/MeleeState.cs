﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{
    private float attackTimer;
    private float attackCooldown = 3;
    private bool canAttack = true;
    
    // reference to enemy
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();

        // if not in melee range - chase player
        if(!enemy.InMeleeRange)
        {
            enemy.ChangeState(new PatrolState());
        }
        // if target is null go to idle state(when for example he jumps away)
        else if(enemy.Target == null)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            canAttack = true;
            attackTimer = 0;
        }

        if (canAttack)
        {
            canAttack = false;
            enemy.MyAnimator.SetTrigger("attack");
        }
    }
}
