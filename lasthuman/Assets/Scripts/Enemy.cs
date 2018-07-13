using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // variable to watch state
    private IEnemyState currentState;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
        ChangeState(new IdleState());
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentState.Execute();
	}

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();

        }

        //change current state to a new state
        currentState = newState;

        // enter new state
        currentState.Enter(this);
    }

    public void Move()
    {
        // to avoid sliding
        MyAnimator.SetFloat("speed", 1);

        transform.Translate( GetDirection() * (movementSpeed * Time.deltaTime) );
    }

    public Vector2 GetDirection()
    {
        // short form of if statement
        return facingRight ? Vector2.right : Vector2.left;
    }
}
